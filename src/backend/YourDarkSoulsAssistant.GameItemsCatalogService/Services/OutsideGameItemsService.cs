using System.Text.Json;
using YourDarkSoulsAssistant.GameItemsCatalogService.DTOs;

namespace YourDarkSoulsAssistant.GameItemsCatalogService.Services;

public class OutsideGameItemsService(HttpClient httpClient)
{
    // Змінюємо тип повернення на Task<string?>, бо ми повертаємо текст (HTML)
    public async Task<string?> GetItemHtmlByNameAsync(string itemName)
    {
        // 1. Формуємо запит та отримуємо відповідь
        var requestUrl = $"https://darksouls.fandom.com/api.php?action=query&format=json&prop=revisions&titles={itemName}&rvprop=content&rvparse&redirects=1";
        var response = await httpClient.GetAsync(requestUrl);
        
        // Переконуємося, що запит був успішним (статус 200-299)
        response.EnsureSuccessStatusCode();

        // 2. Читаємо тіло відповіді як рядок JSON
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // 3. Десеріалізуємо JSON у наші класи
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var apiData = JsonSerializer.Deserialize<WikiRootResponse>(jsonResponse, options);

        // 4. Безпечно витягуємо HTML
        if (apiData?.Query?.Pages != null && apiData.Query.Pages.Count > 0)
        {
            // Беремо першу сторінку зі словника, ігноруючи її ключ (ID)
            var firstPage = apiData.Query.Pages.Values.FirstOrDefault();

            // Якщо є ревізії, повертаємо HTML першої з них
            if (firstPage?.Revisions != null && firstPage.Revisions.Count > 0)
            {
                return firstPage.Revisions[0].HtmlContent;
            }
        }

        // Якщо сторінку не знайдено або HTML відсутній, повертаємо null
        return null;
    }
}