using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Services
{
    public static class ExitService
    {
        private static bool confirmed = false;

        public static bool IsConfirmed
        {
            get { return confirmed; }
        }

        public static async Task ConfirmExitAsync()
        {
            Page? page = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (page == null)
            {
                return;
            }

            bool answer = await page.DisplayAlertAsync(
                "Вихід",
                "Чи дійсно ви хочете завершити роботу з програмою?",
                "Так",
                "Ні");

            if (answer)
            {
                confirmed = true;
                Application.Current?.Quit();
            }
        }
    }
}