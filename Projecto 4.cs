using System;
using System.Collections.Generic;
using System.Linq;

// La clase Contact se mantiene igual, ya que está bien diseñada.
public class Contact
{
    // Propiedades de la clase Contacto
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Telephone { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public bool IsBestFriend { get; set; }

    // Constructor para crear un nuevo contacto
    public Contact(int id, string name, string lastName, string address, string telephone, string email, int age, bool isBestFriend)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Address = address;
        Telephone = telephone;
        Email = email;
        Age = age;
        IsBestFriend = isBestFriend;
    }

    // Método para mostrar los detalles del contacto
    public void DisplayContact()
    {
        string isBestFriendStr = IsBestFriend ? "Sí" : "No";
        Console.WriteLine($"{Id,-5} {Name,-15} {LastName,-15} {Address,-25} {Telephone,-15} {Email,-30} {Age,-5} {isBestFriendStr,-15}");
    }
}

// Nueva clase para gestionar la colección de contactos.
// Esta clase encapsula la lógica de negocio.
public class ContactManager
{
    private Dictionary<int, Contact> contacts;
    private int nextId = 1;

    public ContactManager()
    {
        contacts = new Dictionary<int, Contact>();
    }

    public void AddContact(string name, string lastName, string address, string telephone, string email, int age, bool isBestFriend)
    {
        Contact newContact = new Contact(nextId, name, lastName, address, telephone, email, age, isBestFriend);
        contacts.Add(nextId, newContact);
        nextId++;
        Console.WriteLine("¡Contacto agregado exitosamente!");
    }

    public void ViewContacts()
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No hay contactos en la lista.");
            return;
        }

        Console.WriteLine($"{"ID",-5} {"Nombre",-15} {"Apellido",-15} {"Dirección",-25} {"Teléfono",-15} {"Email",-30} {"Edad",-5} {"Mejor Amigo?",-15}");
        Console.WriteLine(new string('_', 130));

        foreach (var contact in contacts.Values)
        {
            contact.DisplayContact();
        }
    }

    public List<Contact> SearchContact(string searchTerm)
    {
        // Convertir a minúsculas para búsqueda insensible a mayúsculas/minúsculas
        string searchLower = searchTerm.ToLower();
        var foundContacts = contacts.Values.Where(c =>
            c.Name.ToLower().Contains(searchLower) ||
            c.LastName.ToLower().Contains(searchLower)
        ).ToList();

        return foundContacts;
    }

    public Contact GetContactById(int id)
    {
        if (contacts.TryGetValue(id, out Contact contact))
        {
            return contact;
        }
        return null; // Retorna null si no se encuentra el contacto
    }

    public bool UpdateContact(int id, string name, string lastName, string address, string telephone, string email, int age, bool isBestFriend)
    {
        if (contacts.TryGetValue(id, out Contact contactToModify))
        {
            contactToModify.Name = name;
            contactToModify.LastName = lastName;
            contactToModify.Address = address;
            contactToModify.Telephone = telephone;
            contactToModify.Email = email;
            contactToModify.Age = age;
            contactToModify.IsBestFriend = isBestFriend;
            return true;
        }
        return false;
    }

    public bool DeleteContact(int id)
    {
        return contacts.Remove(id);
    }
}

// La clase Program ahora se enfoca en la interacción con el usuario y utiliza el ContactManager.
public class Program
{
    public static void Main(string[] args)
    {
        // Crear una instancia de la clase ContactManager
        ContactManager contactManager = new ContactManager();

        Console.WriteLine("Bienvenido a mi lista de Contactos");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Menú Principal ---");
            Console.WriteLine("1. Agregar Contacto");
            Console.WriteLine("2. Ver Contactos");
            Console.WriteLine("3. Buscar Contactos");
            Console.WriteLine("4. Modificar Contacto");
            Console.WriteLine("5. Eliminar Contacto");
            Console.WriteLine("6. Salir");
            Console.Write("Digite el número de la opción deseada: ");

            int typeOption;
            if (!int.TryParse(Console.ReadLine(), out typeOption))
            {
                Console.WriteLine("Entrada inválida. Por favor, digite un número para la opción.");
                continue;
            }

            switch (typeOption)
            {
                case 1:
                    HandleAddContact(contactManager);
                    break;
                case 2:
                    contactManager.ViewContacts();
                    break;
                case 3:
                    HandleSearchContact(contactManager);
                    break;
                case 4:
                    HandleModifyContact(contactManager);
                    break;
                case 5:
                    HandleDeleteContact(contactManager);
                    break;
                case 6:
                    running = false;
                    Console.WriteLine("¡Gracias por usar la aplicación! ¡Hasta luego!");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, seleccione una opción del 1 al 6.");
                    break;
            }
        }
    }

    // Métodos para manejar la entrada del usuario, delegando la lógica a ContactManager
    private static void HandleAddContact(ContactManager manager)
    {
        Console.WriteLine("\n--- Agregar Nuevo Contacto ---");
        Console.Write("Digite el nombre de la persona: ");
        string name = Console.ReadLine();
        Console.Write("Digite el apellido de la persona: ");
        string lastname = Console.ReadLine();
        Console.Write("Digite la dirección: ");
        string address = Console.ReadLine();
        Console.Write("Digite el teléfono de la persona: ");
        string phone = Console.ReadLine();
        Console.Write("Digite el email de la persona: ");
        string email = Console.ReadLine();

        int age = GetIntInput("Digite la edad de la persona en números: ");
        bool isBestFriend = GetYesNoInput("Especifique si es mejor amigo (1. Sí, 2. No): ");

        manager.AddContact(name, lastname, address, phone, email, age, isBestFriend);
    }

    private static void HandleSearchContact(ContactManager manager)
    {
        Console.WriteLine("\n--- Buscar Contacto ---");
        Console.Write("Ingrese el nombre o apellido del contacto a buscar: ");
        string searchTerm = Console.ReadLine();

        var foundContacts = manager.SearchContact(searchTerm);

        if (foundContacts.Count == 0)
        {
            Console.WriteLine($"No se encontraron contactos que coincidan con '{searchTerm}'.");
        }
        else
        {
            Console.WriteLine("\n--- Resultados de la Búsqueda ---");
            Console.WriteLine($"{"ID",-5} {"Nombre",-15} {"Apellido",-15} {"Dirección",-25} {"Teléfono",-15} {"Email",-30} {"Edad",-5} {"Mejor Amigo?",-15}");
            Console.WriteLine(new string('_', 130));

            foreach (var contact in foundContacts)
            {
                contact.DisplayContact();
            }
        }
    }

    private static void HandleModifyContact(ContactManager manager)
    {
        Console.WriteLine("\n--- Modificar Contacto ---");
        int idToModify = GetIntInput("Ingrese el ID del contacto que desea modificar: ");

        Contact contactToModify = manager.GetContactById(idToModify);
        if (contactToModify != null)
        {
            Console.WriteLine($"Modificando contacto con ID: {contactToModify.Id} - {contactToModify.Name} {contactToModify.LastName}");
            Console.WriteLine("Deje en blanco un campo si no desea modificarlo.");

            string newName = GetStringInput($"Nuevo nombre (actual: {contactToModify.Name}): ");
            if (!string.IsNullOrEmpty(newName)) contactToModify.Name = newName;

            string newLastName = GetStringInput($"Nuevo apellido (actual: {contactToModify.LastName}): ");
            if (!string.IsNullOrEmpty(newLastName)) contactToModify.LastName = newLastName;

            string newAddress = GetStringInput($"Nueva dirección (actual: {contactToModify.Address}): ");
            if (!string.IsNullOrEmpty(newAddress)) contactToModify.Address = newAddress;

            string newPhone = GetStringInput($"Nuevo teléfono (actual: {contactToModify.Telephone}): ");
            if (!string.IsNullOrEmpty(newPhone)) contactToModify.Telephone = newPhone;

            string newEmail = GetStringInput($"Nuevo email (actual: {contactToModify.Email}): ");
            if (!string.IsNullOrEmpty(newEmail)) contactToModify.Email = newEmail;

            int newAge = GetIntInput($"Nueva edad (actual: {contactToModify.Age}). Deje en blanco si no desea modificar: ", allowEmpty: true);
            if (newAge > 0) contactToModify.Age = newAge;

            bool? newIsBestFriend = GetYesNoInput($"¿Es mejor amigo? (1. Sí, 2. No) (actual: {(contactToModify.IsBestFriend ? "Sí" : "No")}). Deje en blanco si no desea modificar: ", allowEmpty: true);
            if (newIsBestFriend.HasValue) contactToModify.IsBestFriend = newIsBestFriend.Value;

            Console.WriteLine("¡Contacto modificado exitosamente!");
        }
        else
        {
            Console.WriteLine($"No se encontró ningún contacto con el ID {idToModify}.");
        }
    }

    private static void HandleDeleteContact(ContactManager manager)
    {
        Console.WriteLine("\n--- Eliminar Contacto ---");
        int idToDelete = GetIntInput("Ingrese el ID del contacto que desea eliminar: ");

        if (manager.DeleteContact(idToDelete))
        {
            Console.WriteLine($"¡Contacto con ID {idToDelete} eliminado exitosamente!");
        }
        else
        {
            Console.WriteLine($"No se encontró ningún contacto con el ID {idToDelete}.");
        }
    }

    // Métodos de utilidad para manejar la entrada del usuario de manera más limpia
    private static int GetIntInput(string prompt, bool allowEmpty = false)
    {
        int value;
        string input;
        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) && allowEmpty) return -1;
            if (int.TryParse(input, out value) && value > 0) return value;
            Console.WriteLine("Entrada inválida. Por favor, digite un número válido.");
        }
    }

    private static bool GetYesNoInput(string prompt, bool allowEmpty = false)
    {
        int option;
        string input;
        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) && allowEmpty) return false;
            if (int.TryParse(input, out option) && (option == 1 || option == 2))
            {
                return option == 1;
            }
            Console.WriteLine("Entrada inválida. Por favor, digite 1 para Sí o 2 para No.");
        }
    }

    private static string GetStringInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}