using System;
using System.Collections.Generic;
using System.Linq;

// 1. Clase Paciente: Representa la información de un paciente.
public class Paciente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set; }
    public string Enfermedad { get; set; }
    public bool Internado { get; set; }

    // Constructor para inicializar un nuevo paciente
    public Paciente(int id, string nombre, string apellido, string telefono, string email, string direccion, string enfermedad, bool internado)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Email = email;
        Direccion = direccion;
        Enfermedad = enfermedad;
        Internado = internado;
    }

    // Método para mostrar los datos del paciente en una línea.
    public void MostrarDatos()
    {
        string estadoInternado = Internado ? "Sí" : "No";
        Console.WriteLine($"{Id,-5} {Nombre,-15} {Apellido,-15} {Telefono,-15} {Email,-25} {Enfermedad,-20} {estadoInternado,-10}");
    }
}

// 2. Clase GestorPacientes: Encapsula la lógica de negocio.
public class GestorPacientes
{
    private Dictionary<int, Paciente> pacientes = new Dictionary<int, Paciente>();
    private int nextId = 1;

    // Método para agregar un paciente, ahora con los nuevos campos.
    public void AgregarPaciente(string nombre, string apellido, string telefono, string email, string direccion, string enfermedad, bool internado)
    {
        Paciente nuevoPaciente = new Paciente(nextId, nombre, apellido, telefono, email, direccion, enfermedad, internado);
        pacientes.Add(nextId, nuevoPaciente);
        nextId++;
        Console.WriteLine("\n¡Paciente registrado exitosamente!");
    }

    // Método para mostrar todos los pacientes, incluyendo los nuevos campos.
    public void VerPacientes()
    {
        if (pacientes.Count == 0)
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        Console.WriteLine($"{"ID",-5} {"Nombre",-15} {"Apellido",-15} {"Teléfono",-15} {"Email",-25} {"Enfermedad",-20} {"Internado",-10}");
        Console.WriteLine(new string('-', 120));
        foreach (var paciente in pacientes.Values)
        {
            paciente.MostrarDatos();
        }
    }

    // Método para buscar pacientes por nombre o apellido
    public List<Paciente> BuscarPacientes(string termino)
    {
        // Esta es la parte del código corregida para garantizar la compatibilidad.
        string terminoEnMinusculas = termino.ToLower();
        return pacientes.Values.Where(p =>
            p.Nombre.ToLower().Contains(terminoEnMinusculas) ||
            p.Apellido.ToLower().Contains(terminoEnMinusculas)
        ).ToList();
    }

    // Método para obtener un paciente por su ID
    public Paciente ObtenerPacientePorId(int id)
    {
        if (pacientes.TryGetValue(id, out Paciente paciente))
        {
            return paciente;
        }
        return null;
    }

    // Método para eliminar un paciente
    public bool EliminarPaciente(int id)
    {
        return pacientes.Remove(id);
    }
}

// 3. Clase Program: Maneja la interacción con el usuario y utiliza GestorPacientes.
public class Program
{
    public static void Main(string[] args)
    {
        GestorPacientes gestor = new GestorPacientes();
        bool running = true;

        Console.WriteLine("--- Bienvenido al Sistema de Registro de Pacientes ---");

        while (running)
        {
            Console.WriteLine("\n--- Menú Principal ---");
            Console.WriteLine("1. Registrar nuevo paciente");
            Console.WriteLine("2. Ver todos los pacientes");
            Console.WriteLine("3. Buscar paciente");
            Console.WriteLine("4. Eliminar paciente");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    RegistrarPaciente(gestor);
                    break;
                case "2":
                    Console.WriteLine("\n--- Lista de Pacientes ---");
                    gestor.VerPacientes();
                    break;
                case "3":
                    BuscarPaciente(gestor);
                    break;
                case "4":
                    EliminarPaciente(gestor);
                    break;
                case "5":
                    running = false;
                    Console.WriteLine("¡Gracias por usar el sistema!");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                    break;
            }
        }
    }

    static void RegistrarPaciente(GestorPacientes gestor)
    {
        Console.WriteLine("\n--- Registrar Nuevo Paciente ---");
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Apellido: ");
        string apellido = Console.ReadLine();
        Console.Write("Teléfono: ");
        string telefono = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Dirección: ");
        string direccion = Console.ReadLine();
        Console.Write("Enfermedad: ");
        string enfermedad = Console.ReadLine();

        bool internado = false;
        Console.Write("¿Debe ser internado? (1. Sí, 2. No): ");
        string internadoOpcion = Console.ReadLine();
        if (internadoOpcion == "1")
        {
            internado = true;
        }

        gestor.AgregarPaciente(nombre, apellido, telefono, email, direccion, enfermedad, internado);
    }

    static void BuscarPaciente(GestorPacientes gestor)
    {
        Console.WriteLine("\n--- Buscar Paciente ---");
        Console.Write("Ingrese nombre o apellido a buscar: ");
        string termino = Console.ReadLine();

        var resultados = gestor.BuscarPacientes(termino);
        if (resultados.Any())
        {
            Console.WriteLine("\n--- Resultados de la Búsqueda ---");
            Console.WriteLine($"{"ID",-5} {"Nombre",-15} {"Apellido",-15} {"Teléfono",-15} {"Email",-25} {"Enfermedad",-20} {"Internado",-10}");
            Console.WriteLine(new string('-', 120));
            foreach (var paciente in resultados)
            {
                paciente.MostrarDatos();
            }
        }
        else
        {
            Console.WriteLine($"No se encontraron pacientes que coincidan con '{termino}'.");
        }
    }

    static void EliminarPaciente(GestorPacientes gestor)
    {
        Console.WriteLine("\n--- Eliminar Paciente ---");
        Console.Write("Ingrese el ID del paciente que desea eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (gestor.EliminarPaciente(id))
            {
                Console.WriteLine($"¡Paciente con ID {id} eliminado exitosamente!");
            }
            else
            {
                Console.WriteLine($"No se encontró ningún paciente con el ID {id}.");
            }
        }
        else
        {
            Console.WriteLine("ID inválido. Por favor, ingrese un número.");
        }
    }
}