using System;
using System.Collections.Generic;
using System.Linq;

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

public class Program
{
    // Usamos un diccionario donde la clave es el ID del contacto y el valor es el objeto Contacto
    static Dictionary<int, Contact> contacts = new Dictionary<int, Contact>();
    static int nextId = 1; // Para generar IDs únicos automáticamente

    public static void Main(string[] args)
    {
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
            // Validación de entrada para la opción del menú
            if (!int.TryParse(Console.ReadLine(), out typeOption))
            {
                Console.WriteLine("Entrada inválida. Por favor, digite un número para la opción.");
                continue; // Vuelve al inicio del bucle
            }

            switch (typeOption)
            {
                case 1:
                    AddContact();
                    break;
                case 2:
                    ViewContacts();
                    break;
                case 3:
                    SearchContact();
                    break;
                case 4:
                    ModifyContact();
                    break;
                case 5:
                    DeleteContact();
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

    // --- Métodos de Gestión de Contactos ---

    static void AddContact()
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

        int age;
        Console.Write("Digite la edad de la persona en números: ");
        while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
        {
            Console.WriteLine("Entrada inválida. Por favor, digite un número válido para la edad.");
            Console.Write("Digite la edad de la persona en números: ");
        }

        bool isBestFriend;
        Console.Write("Especifique si es mejor amigo (1. Sí, 2. No): ");
        int bestFriendOption;
        while (!int.TryParse(Console.ReadLine(), out bestFriendOption) || (bestFriendOption != 1 && bestFriendOption != 2))
        {
            Console.WriteLine("Entrada inválida. Por favor, digite 1 para Sí o 2 para No.");
            Console.Write("Especifique si es mejor amigo (1. Sí, 2. No): ");
        }
        isBestFriend = (bestFriendOption == 1);

        // Crear un nuevo objeto Contacto y añadirlo al diccionario
        Contact newContact = new Contact(nextId, name, lastname, address, phone, email, age, isBestFriend);
        contacts.Add(nextId, newContact);
        nextId++; // Incrementar el ID para el próximo contacto

        Console.WriteLine("¡Contacto agregado exitosamente!");
    }

    static void ViewContacts()
    {
        Console.WriteLine("\n--- Lista de Contactos ---");
        if (contacts.Count == 0)
        {
            Console.WriteLine("No hay contactos en la lista.");
            return;
        }

        Console.WriteLine($"{"ID",-5} {"Nombre",-15} {"Apellido",-15} {"Dirección",-25} {"Teléfono",-15} {"Email",-30} {"Edad",-5} {"Mejor Amigo?",-15}");
        Console.WriteLine(new string('_', 130)); // Línea separadora

        foreach (var contact in contacts.Values)
        {
            contact.DisplayContact();
        }
    }

    static void SearchContact()
    {
        Console.WriteLine("\n--- Buscar Contacto ---");
        Console.Write("Ingrese el nombre o apellido del contacto a buscar: ");
        string searchTerm = Console.ReadLine().ToLower(); // Convertir a minúsculas para búsqueda insensible a mayúsculas/minúsculas

        // Filtrar contactos que contengan el término de búsqueda en nombre o apellido
        var foundContacts = contacts.Values.Where(c =>
            c.Name.ToLower().Contains(searchTerm) ||
            c.LastName.ToLower().Contains(searchTerm)
        ).ToList();

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

    static void ModifyContact()
    {
        Console.WriteLine("\n--- Modificar Contacto ---");
        Console.Write("Ingrese el ID del contacto que desea modificar: ");
        int idToModify;
        if (!int.TryParse(Console.ReadLine(), out idToModify))
        {
            Console.WriteLine("ID inválido. Por favor, digite un número.");
            return;
        }

        // Intentar obtener el contacto del diccionario
        if (contacts.TryGetValue(idToModify, out Contact contactToModify))
        {
            Console.WriteLine($"Modificando contacto con ID: {contactToModify.Id} - {contactToModify.Name} {contactToModify.LastName}");
            Console.WriteLine("Deje en blanco un campo si no desea modificarlo.");

            Console.Write($"Nuevo nombre (actual: {contactToModify.Name}): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                contactToModify.Name = newName;
            }

            Console.Write($"Nuevo apellido (actual: {contactToModify.LastName}): ");
            string newLastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newLastName))
            {
                contactToModify.LastName = newLastName;
            }

            Console.Write($"Nueva dirección (actual: {contactToModify.Address}): ");
            string newAddress = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAddress))
            {
                contactToModify.Address = newAddress;
            }

            Console.Write($"Nuevo teléfono (actual: {contactToModify.Telephone}): ");
            string newPhone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPhone))
            {
                contactToModify.Telephone = newPhone;
            }

            Console.Write($"Nuevo email (actual: {contactToModify.Email}): ");
            string newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                contactToModify.Email = newEmail;
            }

            Console.Write($"Nueva edad (actual: {contactToModify.Age}). Deje en blanco si no desea modificar: ");
            string ageInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ageInput))
            {
                int newAge;
                if (int.TryParse(ageInput, out newAge) && newAge > 0)
                {
                    contactToModify.Age = newAge;
                }
                else
                {
                    Console.WriteLine("Edad inválida, se mantendrá la edad actual.");
                }
            }

            Console.Write($"¿Es mejor amigo? (1. Sí, 2. No) (actual: {(contactToModify.IsBestFriend ? "Sí" : "No")}). Deje en blanco si no desea modificar: ");
            string bestFriendInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(bestFriendInput))
            {
                int bestFriendOption;
                if (int.TryParse(bestFriendInput, out bestFriendOption) && (bestFriendOption == 1 || bestFriendOption == 2))
                {
                    contactToModify.IsBestFriend = (bestFriendOption == 1);
                }
                else
                {
                    Console.WriteLine("Opción de mejor amigo inválida, se mantendrá el estado actual.");
                }
            }

            Console.WriteLine("¡Contacto modificado exitosamente!");
        }
        else
        {
            Console.WriteLine($"No se encontró ningún contacto con el ID {idToModify}.");
        }
    }

    static void DeleteContact()
    {
        Console.WriteLine("\n--- Eliminar Contacto ---");
        Console.Write("Ingrese el ID del contacto que desea eliminar: ");
        int idToDelete;
        if (!int.TryParse(Console.ReadLine(), out idToDelete))
        {
            Console.WriteLine("ID inválido. Por favor, digite un número.");
            return;
        }

        // Intentar eliminar el contacto del diccionario
        if (contacts.Remove(idToDelete))
        {
            Console.WriteLine($"¡Contacto con ID {idToDelete} eliminado exitosamente!");
        }
        else
        {
            Console.WriteLine($"No se encontró ningún contacto con el ID {idToDelete}.");
        }
    }
}