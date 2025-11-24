// See https://aka.ms/new-console-template for more information

using CoreZipCode.Services.Postcode.PostalpincodeInApi;
using CoreZipCode.Services.Postcode.PostcodesIoApi;
using CoreZipCode.Services.ZipCode.SmartyApi;
using CoreZipCode.Services.ZipCode.ViaCepApi;
using Newtonsoft.Json;

static void ViaCepService()
{
    Console.WriteLine("You picked up: ViaCep Service");
    Console.WriteLine("Type your Brazilian Zipcode (eg: 14810100):");
    var zipcode = Console.ReadLine();

    var objViaCepService = new ViaCep();
    Console.Write("Your execute method output is:\n");
    Console.Write(objViaCepService.Execute(zipcode));

    Console.Write("\n\nYour GetAddress method output is:\n");
    var addressModel = objViaCepService.GetAddress<ViaCepAddressModel>(zipcode);
    Console.Write(JsonConvert.SerializeObject(addressModel).ToString());

    Console.Write("\n\nType any key to turn back to menu");
    Console.ReadKey();
}

static void SmartyService()
{
    Console.WriteLine("You picked up: Smarty Service");
    Console.WriteLine("Type your US Zipcode (eg: 95014):");
    var zipcode = Console.ReadLine();

    var authId = ""; // Create an account on https://www.smarty.com/ and put your own.
    var authToken = ""; // Create an account on https://www.smarty.com/ and put your own.

    if (authId.Equals(string.Empty) || authToken.Equals(string.Empty))
    {
        Console.Write("You show provide yours access tokens");
        Console.ReadKey();
        return;
    }

    var objSmartyService = new Smarty(authId, authToken);
    Console.Write("Your Execute method output is:\n");
    Console.Write(objSmartyService.Execute(zipcode));

    Console.Write("\n\nYour GetAddress method output is:\n");
    var addressModel = objSmartyService.GetAddress<List<SmartyModel>>(zipcode);
    Console.Write(JsonConvert.SerializeObject(addressModel).ToString());

    Console.Write("\n\nType any key to turn back to menu");
    Console.ReadKey();
}

static void PostalpincodeInService()
{
    Console.WriteLine("You picked up: PostalpicodeIn Service");
    Console.WriteLine("Type your Indian Postalcode (eg: 744302):");
    var postcode = Console.ReadLine();

    var objPostalpincodeInService = new PostalpincodeIn();
    var result = objPostalpincodeInService.Execute(postcode);
    Console.Write("Your Execute method output is:\n");
    Console.Write(result);

    Console.Write("\nYour GetPostcode method output is:\n");
    var postModel = objPostalpincodeInService.GetPostcode<PostalpincodeInModel>(postcode);
    Console.Write(JsonConvert.SerializeObject(postModel).ToString());

    Console.Write("\n\nType any key to turn back to menu");
    Console.ReadKey();
}

static void PostcodesIoService()
{
    Console.WriteLine("You picked up: Smarty Service");
    Console.WriteLine("Type your UK Postcode (eg: CM81EF):");
    var postcode = Console.ReadLine();

    var objPostcodesIoService = new PostcodesIo();
    var result = objPostcodesIoService.Execute(postcode);
    Console.Write("Your Execute output is:\n");
    Console.Write(result);

    Console.Write("\n\nYour GetPostcode method output is:\n");
    var postModel = objPostcodesIoService.GetPostcode<PostcodesIoModel>(postcode);
    Console.Write(JsonConvert.SerializeObject(postModel).ToString());

    Console.Write("\n\nType any key to turn back to menu");
    Console.ReadKey();
}

static bool PrintMenu()
{
    Console.Clear();
    Console.WriteLine("  ---------------------------------------");
    Console.WriteLine(" |****  CoreZipCode Demo Application ****|");
    Console.WriteLine("  ---------------------------------------");
    Console.WriteLine(" 1 - ViaCep Service");
    Console.WriteLine(" 2 - Smarty Service");
    Console.WriteLine(" 3 - PostalpincodeIn Service");
    Console.WriteLine(" 4 - PostcodesIo Service");
    Console.WriteLine(" 5 - Get out");
    Console.WriteLine("\n");
    Console.WriteLine(" Type your choice:");

    switch (Console.ReadLine())
    {
        case "1":
            ViaCepService();
            return true;
        case "2":
            SmartyService();
            return true;
        case "3":
            PostalpincodeInService();
            return true;
        case "4":
            PostcodesIoService();
            return true;
        case "5":
            return false;
        default:
            return true;
    }
}

var showMenu = true;
while (showMenu)
{
    showMenu = PrintMenu();
}
