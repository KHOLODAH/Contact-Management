


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime CreationDate { get; set; }
}





public interface IContactRepository
{
    void Add();
    void Update();
    void Delete();
    void GetById();
    void DisplayAll();
    
    IEnumerable<Contact> GetAll();
    void SaveToFile(string filename = "contacts.json");
    void LoadFromFile(string filename = "contacts.json");
}

public class JsonContactRepository : IContactRepository
{
    private Dictionary<int, Contact> contacts = new Dictionary<int, Contact>();
    private int nextId = 1;

    public void Add()
    {
        var c = new Contact();
        Console.Write("Name: ");
        c.Name = Console.ReadLine();
        Console.Write("Phone: ");
        c.Phone = Console.ReadLine();
        Console.Write("Email: ");
        c.Email = Console.ReadLine();
        c.Id = nextId++;
        c.CreationDate = DateTime.Now;
        contacts[c.Id] = c;
        Console.WriteLine($"Contact added with ID {c.Id}");

    }

    public void Update()
    {

        Console.Write("Enter ID to edit: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }
        if (!contacts.TryGetValue(id, out var c))
        {
            Console.WriteLine("Contact not found");
            return;
        }

        Console.Write("New Name (leave empty to skip): ");
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            c.Name = input;
        }
        Console.Write("New Phone: ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            c.Phone = input;
        }
        Console.Write("New Email: ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            c.Email = input;
        }

        if (contacts.ContainsKey(c.Id))
            contacts[c.Id] = c;
        Console.WriteLine("Contact updated successfully!");



    }

    public void Delete() {


        Console.Write("Enter ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }
        if (contacts.Remove(id))
        {
            Console.WriteLine("Deleted successfully");
        }
        else
        {
            Console.WriteLine("Contact not found");
        }



    }

    public void GetById()
    {
        Console.Write("Enter ID to view: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }

        if (contacts.TryGetValue(id, out var c))
        {

            Console.WriteLine($"ID: {c.Id}");
            Console.WriteLine($"Name: {c.Name}");
            Console.WriteLine($"Phone: {c.Phone}");
            Console.WriteLine($"Email: {c.Email}");
            Console.WriteLine($"Created: {c.CreationDate}");
        }
        else
        {
            Console.WriteLine("Contact not found");
        }
    }

    public void DisplayAll() {

        foreach (var c in contacts.Values)
            Console.WriteLine(
                $"ID:{c.Id}  Name: {c.Name}  phone: {c.Phone}  Email: {c.Email}  Created: {c.CreationDate}");

}



    public IEnumerable<Contact> GetAll()
    {
        return contacts.Values; 
    }


        

    public void SaveToFile(string filename = "contacts.json")
    {
        var list = contacts.Values.ToList();
        string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filename, json);
        Console.WriteLine("Contacts saved successfully!");
    }

    public void LoadFromFile(string filename = "contacts.json")
    {
        if (!File.Exists(filename)) return;
        string json = File.ReadAllText(filename);
        var list = JsonSerializer.Deserialize<List<Contact>>(json);
        contacts.Clear();
        nextId = 1;
        foreach (var c in list)
        {
            contacts[c.Id] = c;
            if (c.Id >= nextId) nextId = c.Id + 1;
        }
        Console.WriteLine("Contacts loaded successfully!");
    }
} 







public interface IContactService
{
    IEnumerable<Contact> SearchByName(string name);
    IEnumerable<Contact> SearchByPhone(string phone);
   
    IEnumerable<Contact> FilterCreatedAfter(DateTime date);
    IEnumerable<Contact> FilterByFirstName(string firstName);
}

public class ContactService : IContactService
{
    private readonly IContactRepository repository;

    public ContactService(IContactRepository repo)
    {
        repository = repo;
    }

    public IEnumerable<Contact> SearchByName(string name)
    {
      return  repository.GetAll().Where(c => c.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
    }
    public IEnumerable<Contact> SearchByPhone(string phone)
    {
      return  repository.GetAll().Where(c => c.Phone == phone);
    }


    public IEnumerable<Contact> FilterCreatedAfter(DateTime date) {
       return repository.GetAll().Where(c => c.CreationDate > date);
    }
    public IEnumerable<Contact> FilterByFirstName(string firstName)
    {
        return repository.GetAll()
             .Where(c => c.Name != null && c.Name.Split(' ')[0]
             .Equals(firstName, StringComparison.OrdinalIgnoreCase));
    }
}






public class ConsoleUI
{
    private readonly IContactRepository repo;
    private readonly IContactService service;

    public ConsoleUI(IContactRepository r, IContactService s)
    {
        repo = r;
        service = s;
        repo.LoadFromFile();
    }

    public void Run()
    {
        
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== Contact Management System ===");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. Edit Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. View Contact");
            Console.WriteLine("5. List All Contacts");
            Console.WriteLine("6. Search");
            Console.WriteLine("7. Filter");
            Console.WriteLine("8. Save Contacts");
            Console.WriteLine("9. Exit");
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1": repo.Add(); break;
                case "2": repo.Update(); break;
                case "3": repo.Delete(); break;
                case "4": repo.GetById(); ; break;
                case "5": repo.DisplayAll(); break;
                case "6": SearchMenu(); break;
                case "7": FilterMenu(); break;
                case "8": repo.SaveToFile();  break;
                case "9": exit = true; repo.SaveToFile(); break;
                default: Console.WriteLine("Invalid choice!"); break;
            }
        }
    }

   

   



   

   

    private void SearchMenu()
    {
        Console.WriteLine("Search by: 1- Name, 2- Phone");
        Console.Write("Choice: "); string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.Write("Enter name: ");
                foreach (var c in service.SearchByName(Console.ReadLine()))
                    Console.WriteLine($"{c.Id}: {c.Name} - {c.Phone} - {c.Email}");
                break;
            case "2":
                Console.Write("Enter phone: ");
                foreach (var c in service.SearchByPhone(Console.ReadLine()))
                    Console.WriteLine($"{c.Id}: {c.Name} - {c.Phone} - {c.Email}");
                break;
            default: Console.WriteLine("Invalid choice"); break;
        }
    }

    private void FilterMenu()
    {
        Console.WriteLine("Filter options:  1- Created After Date  2- First Name");
        Console.Write("Choice: "); string choice = Console.ReadLine();
        switch (choice)
        {
            
            case "1":
                Console.Write("Enter date (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    foreach (var c in service.FilterCreatedAfter(date))
                        Console.WriteLine($"{c.Id}: {c.Name} - {c.Phone} - {c.Email} - Created: {c.CreationDate}");
                else Console.WriteLine("Invalid date");
                break;
            case "2":
                Console.Write("Enter first name: ");
                foreach (var c in service.FilterByFirstName(Console.ReadLine()))
                    Console.WriteLine($"{c.Id}: {c.Name} - {c.Phone} - {c.Email}");
                break;
            default: Console.WriteLine("Invalid choice"); break;
        }
    }

    
}







class Program
{
    static void Main()
    {
        IContactRepository repo = new JsonContactRepository();
        IContactService service = new ContactService(repo);
        var ui = new ConsoleUI(repo, service);
        ui.Run();
    }
}