using System.Text.Json;

#region Vars

List<string> contacts = (await File.ReadAllLinesAsync("rubrica.json"))
    .ToList();

#endregion

#region Methods

void PrintContact(string contact)
{
    ContactModel contactModel = JsonSerializer.Deserialize<ContactModel>(contact);
    
    Console.WriteLine($"{contactModel.Nome} {contactModel.Cognome}: {contactModel.Numero}");
}

#endregion

#region Esercizio 1

string firstContact = contacts.First();

// PrintContact(firstContact);

#endregion

#region Esercizio 2

void PrintAllContacts() =>
    contacts.ForEach(contact =>
        PrintContact(contact));

#endregion

#region Models

class ContactModel
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Numero { get; set; }
}

#endregion