using System.Text.Json;

#region Vars

IAsyncEnumerable<string> contacts = File.ReadLinesAsync("rubrica.json");

string FormatContact(string json)
{
    ContactModel contactModel = JsonSerializer.Deserialize<ContactModel>(json) ??
        throw new FormatException("Valore Invalido!");

    return $"{contactModel.Nome} {contactModel.Cognome}: {contactModel.Numero}";
}

IAsyncEnumerable<string> contactsLoaded = contacts.Select(FormatContact);

#endregion

#region Methods

async Task SearchContactAsync(string toSearch)
{
    await contactsLoaded
        .Where(c => c.Contains(toSearch))
        .ForEachAsync(c => Console.WriteLine(c));
}

//async Task AddContact(List<string> args)
async Task AddContactAsync(string? name, string? surname, string? number)
{
    if (name == null)
    {
        Console.Write("Nome: ");
        name = Console.ReadLine() ?? "Undefined";
    }
    
    if (surname == null)
    {
        Console.Write("Cognome: ");
        surname = Console.ReadLine() ?? "Undefined";
    }
    
    if (number == null)
    {
        Console.Write("Numero: ");
        number = Console.ReadLine() ?? "Undefined";
    }
    
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

async Task RemoveContactAsync(string data)
{
    IAsyncEnumerable<(string Contact, int Index)> currentContacs = contactsLoaded
        .Select((c, i) => (c, i))
        .Where(x => x.c.Contains(data));

    if (await currentContacs.AnyAsync())
        Console.WriteLine($"{data} non trovato.");
    else if (await currentContacs.CountAsync() == 1)
    {
        (string contact, int toRemoveIndex) = await currentContacs.FirstAsync();

        using FileStream file = File.OpenWrite("rubrica.json");
        using StreamWriter writer = new StreamWriter(file);
        await foreach ((string json, int index) in contacts.Select((c, i) => (c, i)))
        {
            if (index == toRemoveIndex)
                continue;

            await writer.WriteLineAsync(json);
        }
        
        Console.WriteLine($"Cancellato {contact}");
    }
    else
        Console.WriteLine($"Nome Ambiguo:\n{string.Join("\n", currentContacs.Select(x => x.Contact))}");
}

#endregion

#region Esercizio 1

/* string firstContact = contactsLoaded.First();

Console.WriteLine(firstContact); */

#endregion

#region Esercizio 2

void PrintAllContacts() =>
    contactsLoaded.ForEach(contact =>
        Console.WriteLine(contact));

#endregion

#region Esercizio 3/4/5/7

string cmd, target = "";

if (args.Length == 0)
{
    Console.Write("Non sono stati specificati argomenti, scegli il comando: lista, cerca, nuovo, cancella\nComando -> ");

    cmd = Console.ReadLine()!;
    
    
    if (cmd == "cerca")
    {
        Console.Write("Cerca -> ");

        target = Console.ReadLine()!;
    }
    else if (cmd == "cancella")
    {
        Console.Write("Chi vuoi cancellare? -> ");

        target = Console.ReadLine()!;
    }
}
else
{
    cmd = args[0];

    if (args[0] is "cerca" or "cancella")
    {
        target = args.ElementAtOrDefault(1) ?? throw new Exception("Argomento non specificato!");
    }
}


if (cmd == "lista")
    PrintAllContacts();
else if (cmd == "cerca")
    await SearchContactAsync(target);
else if (cmd == "nuovo")
    await AddContactAsync(args.ElementAtOrDefault(1), args.ElementAtOrDefault(2), args.ElementAtOrDefault(3));
else if (cmd == "cancella")
    await RemoveContactAsync(target);

#endregion

#region Models

class ContactModel
{
    public string? Nome { get; set; }
    public string? Cognome { get; set; }
    public string? Numero { get; set; }
}

#endregion