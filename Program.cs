using System.Text.Json;

#region Vars

List<string> contacts = (await File.ReadAllLinesAsync("rubrica.json"))
    .ToList();

List<string> contactsLoaded = contacts.Select(contact =>
{
    ContactModel contactModel = JsonSerializer.Deserialize<ContactModel>(contact);

    return $"{contactModel.Nome} {contactModel.Cognome}: {contactModel.Numero}";
}).ToList();

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

#region Esercizio 3

if (args.Length > 0)
{
    if (args[0] == "lista")
        PrintAllContacts();
    else if (args[0] == "cerca")
    {
        // TODO: Check se args[1] non esiste
        
        contactsLoaded.ForEach(contact =>
        {
            if (contact.Contains(args[1]))
                Console.WriteLine(contact);
        });
    }
}

#endregion

#region Esercizio 4

if (args.Length == 0)
{
    Console.Write("Non sono stati specificati argomenti, scegli il comando: lista, cerca\nComando -> ");

    string cmd = Console.ReadLine();
    
    if (cmd == "lista")
        PrintAllContacts();
    else if (cmd == "cerca")
    {
        Console.Write("Cerca -> ");

        string toSearch = Console.ReadLine();
        
        contactsLoaded.ForEach(contact =>
        {
            if (contact.Contains(toSearch))
                Console.WriteLine(contact);
        });
    }
}

#endregion

#region Models

class ContactModel
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Numero { get; set; }
}

#endregion