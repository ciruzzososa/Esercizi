using System.Text.Json;

#region Vars

List<string> contacts = (await File.ReadAllLinesAsync("rubrica.json"))
    .ToList();

List<string> contactsLoaded = contacts.Select(contact =>
{
    ContactModel contactModel = JsonSerializer.Deserialize<ContactModel>(contact) ??
        throw new FormatException("Valore Invalido!");

    return $"{contactModel.Nome} {contactModel.Cognome}: {contactModel.Numero}";
}).ToList();

#endregion

#region Methods

void SearchContact(string toSearch)
{
    contactsLoaded.ForEach(contact =>
    {
        if (contact.Contains(toSearch))
            Console.WriteLine(contact);
    });
}

async Task AddContact()
{
    Console.Write("Nome: ");
    string name = Console.ReadLine();
        
    Console.Write("Cognome: ");
    string surname = Console.ReadLine();
        
    Console.Write("Numero: ");
    string number = Console.ReadLine();

    
    var ctc = new ContactModel
    {
        Nome = name,
        Cognome = surname,
        Numero = number
    };

    string newContact = JsonSerializer.Serialize(ctc);

    await File.AppendAllTextAsync("rubrica.json", $"\n{newContact}");
        
    Console.WriteLine("Contatto aggiunto!");
}

#endregion

#region Esercizio 1

string firstContact = contactsLoaded.First();

// Console.WriteLine(firstContact);

#endregion

#region Esercizio 2

void PrintAllContacts() =>
    contactsLoaded.ForEach(contact =>
        Console.WriteLine(contact));

#endregion

#region Esercizio 3/4/5

string cmd, toSearch = "";

if (args.Length == 0)
{
    Console.Write("Non sono stati specificati argomenti, scegli il comando: lista, cerca, nuovo\nComando -> ");

    cmd = Console.ReadLine();
    
    
    if (cmd == "cerca")
    {
        Console.Write("Cerca -> ");

        toSearch = Console.ReadLine();
    }
}
else
{
    cmd = args[0];
    
    if (args.Length > 1)
        toSearch = args[1];
    else
    {
        if (cmd == "cerca")
            throw new Exception("Argomento non specificato!");
    }
}


if (cmd == "lista")
    PrintAllContacts();
else if (cmd == "cerca")
    SearchContact(toSearch);
else if (cmd == "nuovo")
    await AddContact();

#endregion

#region Models

class ContactModel
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Numero { get; set; }
}

#endregion