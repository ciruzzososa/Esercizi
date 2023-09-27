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

async Task AddContact(List<string> args)
{
    string name, surname, number;
    
    args.RemoveAt(0);

    if (args.Count < 1)
    {
        Console.Write("Nome: ");
        name = Console.ReadLine();
    }
    else
        name = args[0];
    
    if (args.Count < 2)
    {
        Console.Write("Cognome: ");
        surname = Console.ReadLine();
    }
    else
        surname = args[1];
    
    if (args.Count < 3)
    {
        Console.Write("Numero: ");
        number = Console.ReadLine();
    }
    else
        number = args[2];

    
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
    await AddContact(args.ToList());

#endregion

#region Models

class ContactModel
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Numero { get; set; }
}

#endregion