// See https://aka.ms/new-console-template for more information

using CoreZipCode.Result;
using CoreZipCode.Services.Postcode.PostalpincodeInApi;
using CoreZipCode.Services.Postcode.PostcodesIoApi;
using CoreZipCode.Services.ZipCode.SmartyApi;
using CoreZipCode.Services.ZipCode.ViaCepApi;
using Newtonsoft.Json;

Console.Title = "CoreZipCode Demo";

static async Task ViaCepServiceAsync()
{
    Console.WriteLine("You picked up: ViaCep Service");
    Console.WriteLine("Type your Brazilian Zipcode (eg: 14810100):");
    var zipcode = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(zipcode))
    {
        Console.WriteLine("Invalid zipcode.");
        Pause();
        return;
    }

    var service = new ViaCep();
    var resultByZip = await service.GetAddressAsync<ViaCepAddressModel>(zipcode);
    await HandleResultAsync(resultByZip, "Address found by ZIP code!", "ZIP code not found or API error.");

    if (resultByZip.IsSuccess && resultByZip.Value != null)
    {
        var address = resultByZip.Value;

        // Some fields may be empty – use fallback values for the demo
        string state = string.IsNullOrWhiteSpace(address.State) ? "SP" : address.State;
        string city = string.IsNullOrWhiteSpace(address.City) ? "São Paulo" : address.City;
        string street = string.IsNullOrWhiteSpace(address.Address1)
            ? "Avenida Paulista"                     // fallback street for demo
            : address.Address1.Split(',')[0];      // take only the first part

        Console.WriteLine("\n=== Searching the same area by State + City + Street ===");
        Console.WriteLine($"State : {state}");
        Console.WriteLine($"City  : {city}");
        Console.WriteLine($"Street: {street}");

        // ViaCep also supports reverse lookup (state/city/street → list of addresses)
        var resultByAddress = await service.ListAddressesAsync<ViaCepAddressModel>(state, city, street);
        await HandleResultAsync(resultByAddress,
            $"Found {(resultByAddress.Value!).Count} address(es) by State+City+Street!",
            "No address found with the given State+City+Street.");
    }
}

static async Task SmartyServiceAsync()
{
    Console.WriteLine("You picked up: Smarty Service");
    Console.WriteLine("Type your US Zipcode (eg: 95014):");
    var zipcode = Console.ReadLine()?.Trim();

    var authId = "5c19485e-306d-c4e9-ab20-4b30cb926420"; // Create an account on https://www.smarty.com/ and put your own.
    var authToken = "t7Uvm5G5ox8kalbJyMgC"; // Create an account on https://www.smarty.com/ and put your own.

    if (string.IsNullOrWhiteSpace(authId) || string.IsNullOrWhiteSpace(authToken))
    {
        Console.WriteLine("Error: Smarty credentials are missing. Fill authId/authToken in the code.");
        Pause();
        return;
    }

    if (string.IsNullOrWhiteSpace(zipcode))
    {
        Console.WriteLine("Invalid ZIP code.");
        Pause();
        return;
    }

    var service = new Smarty(authId, authToken);

    var resultByZip = await service.GetAddressAsync<List<SmartyModel>>(zipcode);
    await HandleResultAsync(resultByZip, "Addresses found by ZIP code!", "ZIP code not found or API error.");

    if (resultByZip.IsSuccess && resultByZip.Value != null && resultByZip.Value.Count > 0)
    {
        var firstResult = resultByZip.Value[0];

        string state = firstResult.CityStates?[0]?.StateAbbreviation ?? "CA";
        string city  = firstResult.CityStates?[0]?.City               ?? "Beverly Hills";
        string street = "M";

        Console.WriteLine("\n=== Searching the same area by State + City + Street ===");
        Console.WriteLine($"State : {state}");
        Console.WriteLine($"City  : {city}");
        Console.WriteLine($"Street: {street} (demo street)");

        // Use ListAddressesAsync (calls Smarty street-address endpoint)
        var resultByAddress = await service.ListAddressesAsync<SmartyParamsModel>(state, city, street);

        await HandleResultAsync(
            resultByAddress,
            $"Found {(resultByAddress.Value!).Count} matching address(es)!",
            "No addresses found for the given street."
        );
    }
}

static async Task PostalpincodeInServiceAsync()
{
    Console.WriteLine("You picked up: PostalpicodeIn Service");
    Console.WriteLine("Type your Indian Postalcode (eg: 744302):");

    var pincode = Console.ReadLine()?.Trim();

    var service = new PostalpincodeIn();
    var result = await service.GetPostcodeAsync<PostalpincodeInModel>(pincode);

    await HandleResultAsync(result, "Pincode encontrado!", "Pincode não encontrado ou erro na API.");
}

static async Task PostcodesIoServiceAsync()
{
    Console.WriteLine("You picked up: Smarty Service");
    Console.WriteLine("Type your UK Postcode (eg: CM81EF):");
    var postcode = Console.ReadLine()?.Trim();

    var service = new PostcodesIo();
    var result = await service.GetPostcodeAsync<PostcodesIoModel>(postcode);

    await HandleResultAsync(result, "Postcode encontrado!", "Postcode não encontrado ou erro na API.");
}

static bool PrintMenu()
{
    Console.Clear();
    Console.WriteLine(" ╔══════════════════════════════════════════╗");
    Console.WriteLine(" ║                                          ║");
    Console.WriteLine(" ║        CoreZipCode Demo Application      ║");
    Console.WriteLine(" ║                                          ║");
    Console.WriteLine(" ╚══════════════════════════════════════════╝");
    Console.WriteLine();
    Console.WriteLine(" 1 - ViaCep Service");
    Console.WriteLine(" 2 - Smarty Service");
    Console.WriteLine(" 3 - PostalpincodeIn Service");
    Console.WriteLine(" 4 - PostcodesIo Service");
    Console.WriteLine(" 5 - Get out");
    Console.WriteLine("");
    Console.WriteLine(" Type your choice:");

    return Console.ReadLine() switch
    {
        "1" => RunAndContinue(ViaCepServiceAsync),
        "2" => RunAndContinue(SmartyServiceAsync),
        "3" => RunAndContinue(PostalpincodeInServiceAsync),
        "4" => RunAndContinue(PostcodesIoServiceAsync),
        "5" => false,
        _ => true
    };
}

static void Pause()
{
    Console.Write("\n\nType any key to continue");
    Console.ReadKey(true);
}

static bool RunAndContinue(Func<Task> action)
{
    try
    {
        action().GetAwaiter().GetResult();
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
        Pause();
        return true;
    }
}

static async Task HandleResultAsync<T>(Result<T> result, string successMessage, string notFoundMessage) where T : class
{
    if (result.IsSuccess && result.Value != null)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(successMessage);
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine(JsonConvert.SerializeObject(result.Value, Formatting.Indented));
    }
    else
    {
        var error = result.IsFailure ? result.Error : null;

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Query failed");
        Console.ResetColor();

        if (error != null)
        {
            Console.WriteLine($"Status: {error.StatusCode} ({(int)error.StatusCode})");
            Console.WriteLine($"Mensagem: {error.Message}");

            if (!string.IsNullOrWhiteSpace(error.ResponseBody))
            {
                Console.WriteLine("API response:");
                Console.WriteLine(error.ResponseBody.Length > 1000
                    ? error.ResponseBody.Substring(0, 1000) + "\n[...truncated]"
                    : error.ResponseBody);
            }
        }
        else
        {
            Console.WriteLine(notFoundMessage);
        }
    }

    Pause();
}

var showMenu = true;
while (showMenu)
{
    showMenu = PrintMenu();
}
