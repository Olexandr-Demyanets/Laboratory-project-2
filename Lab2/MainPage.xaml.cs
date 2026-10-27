using System;
using System.Collections.Generic;
using System.IO;
using Lab2.Models;
using Lab2.Services;
using Lab2.Strategies;

namespace Lab2
{
    public partial class MainPage : ContentPage
    {
        private List<ISearchStrategy> strategies = new List<ISearchStrategy>
        {
            new SaxSearchStrategy(),
            new DomSearchStrategy(),
            new LinqSearchStrategy()
        };

        private SearchContext searchContext;
        private AttributeLoader attributeLoader = new AttributeLoader();
        private HtmlTransformer htmlTransformer = new HtmlTransformer();

        private Dictionary<string, Picker> filterPickers = new Dictionary<string, Picker>();

        private string xmlPath = "";
        private string anyValue = "(будь-яке)";

        private FilePickerFileType xmlFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".xml" } },
                { DevicePlatform.Android, new[] { "text/xml", "application/xml" } }
            });

        public MainPage()
        {
            InitializeComponent();

            searchContext = new SearchContext(strategies[0]);

            StrategyPicker.ItemsSource = strategies;
            StrategyPicker.ItemDisplayBinding = new Binding("Name");
            StrategyPicker.SelectedIndex = 0;
        }


        private async void OnPickFileClicked(object? sender, EventArgs e)
        {
            FileResult? file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Оберіть XML-файл",
                FileTypes = xmlFileType
            });

            if (file == null)
            {
                return;
            }

            try
            {
                Dictionary<string, List<string>> values = attributeLoader.LoadAttributeValues(file.FullPath);
                xmlPath = file.FullPath;
                FileLabel.Text = file.FileName;
                BuildFilters(values);
                OutputEditor.Text = "Файл завантажено. Оберіть параметри пошуку і натисніть «Пошук».";
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", "Не вдалося прочитати файл: " + ex.Message, "OK");
            }
        }

        private void OnStrategyChanged(object? sender, EventArgs e)
        {
            if (StrategyPicker.SelectedItem is ISearchStrategy strategy)
            {
                searchContext.SetStrategy(strategy);
            }
        }

        private async void OnSearchClicked(object? sender, EventArgs e)
        {
            if (xmlPath == "")
            {
                await DisplayAlertAsync("Помилка", "Спочатку оберіть XML-файл", "OK");
                return;
            }

            try
            {
                List<Student> found = searchContext.Search(xmlPath, BuildCriteria());
                OutputEditor.Text = FormatResults(found);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", "Помилка під час пошуку: " + ex.Message, "OK");
            }
        }

        private async void OnTransformClicked(object? sender, EventArgs e)
        {
            if (xmlPath == "")
            {
                await DisplayAlertAsync("Помилка", "Спочатку оберіть XML-файл", "OK");
                return;
            }

            try
            {
                string htmlPath = Path.ChangeExtension(xmlPath, ".html");

                using (Stream xslStream = await FileSystem.OpenAppPackageFileAsync("student.xsl"))
                {
                    htmlTransformer.Transform(xmlPath, xslStream, htmlPath);
                }

                OutputEditor.Text = "HTML-файл збережено:\n" + htmlPath;
                await Launcher.Default.OpenAsync(new OpenFileRequest("Результат", new ReadOnlyFile(htmlPath)));
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", "Не вдалося створити HTML: " + ex.Message, "OK");
            }
        }

        private void OnClearClicked(object? sender, EventArgs e)
        {
            OutputEditor.Text = "";
            KeywordEntry.Text = "";

            foreach (Picker picker in filterPickers.Values)
            {
                picker.SelectedIndex = 0;
            }
        }

        private async void OnExitClicked(object? sender, EventArgs e)
        {
            await ExitService.ConfirmExitAsync();
        }

        private void BuildFilters(Dictionary<string, List<string>> values)
        {
            FiltersPanel.Children.Clear();
            filterPickers.Clear();

            foreach (var pair in values)
            {
                Picker picker = new Picker { Title = pair.Key };
                picker.Items.Add(anyValue);

                foreach (string value in pair.Value)
                {
                    picker.Items.Add(value);
                }

                picker.SelectedIndex = 0;

                FiltersPanel.Children.Add(new Label { Text = pair.Key, FontSize = 12 });
                FiltersPanel.Children.Add(picker);
                filterPickers[pair.Key] = picker;
            }
        }

        private SearchCriteria BuildCriteria()
        {
            SearchCriteria criteria = new SearchCriteria();

            foreach (var pair in filterPickers)
            {
                Picker picker = pair.Value;
                if (picker.SelectedIndex > 0)
                {
                    criteria.Filters[pair.Key] = picker.Items[picker.SelectedIndex];
                }
            }

            criteria.Keyword = (KeywordEntry.Text ?? "").Trim();
            return criteria;
        }

        private string FormatResults(List<Student> students)
        {
            string text = "Спосіб аналізу: " + searchContext.StrategyName + "\n";
            text += "Знайдено студентів: " + students.Count + "\n\n";

            foreach (Student student in students)
            {
                text += student.ToString() + "\n";
            }

            return text;
        }
    }
}