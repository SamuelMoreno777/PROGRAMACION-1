// Program.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using AutolavadoApp.Models;
using AutolavadoApp.Data;

class Program
{
    // Instancia estática de DbManager para que todos los métodos la compartan.
    private static DbManager dbManager = new DbManager();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        bool running = true;
        while (running)
        {
            ShowMainMenu();
            string option = GetInput("Por favor, seleccione una opción: ");
            switch (option)
            {
                case "1": ManageClientes(); break;
                case "2": ManageVehiculos(); break;
                case "3": ManageEmpleados(); break;
                case "4": ManageTiposLavado(); break;
                case "5":
                    running = false;
                    Console.WriteLine("\n¡Gracias por usar nuestro sistema! ¡Hasta pronto!");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                    break;
            }
            if (running)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static string GetInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    private static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("--- BIENVENIDO AL SISTEMA DE AUTOLAVADO ---");
        Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
        Console.WriteLine("1. Gestión de Clientes");
        Console.WriteLine("2. Gestión de Vehículos");
        Console.WriteLine("3. Gestión de Empleados");
        Console.WriteLine("4. Gestión de Servicios (Tipos de Lavado)");
        Console.WriteLine("5. Salir");
    }

    // --- Métodos de gestión de Clientes ---
    private static void ManageClientes()
    {
        bool manage = true;
        while (manage)
        {
            Console.Clear();
            Console.WriteLine("--- MENÚ DE GESTIÓN DE CLIENTES ---");
            Console.WriteLine("1. Ver todos los clientes");
            Console.WriteLine("2. Agregar un nuevo cliente");
            Console.WriteLine("3. Actualizar un cliente existente");
            Console.WriteLine("4. Eliminar un cliente");
            Console.WriteLine("5. Volver al menú principal");

            string option = GetInput("Seleccione una opción: ");
            switch (option)
            {
                case "1": ViewAllClientes(); break;
                case "2": AddNuevoCliente(); break;
                case "3": UpdateCliente(); break;
                case "4": DeleteCliente(); break;
                case "5": manage = false; break;
                default: Console.WriteLine("Opción no válida. Intente de nuevo."); break;
            }
            if (manage)
            {
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey();
            }
        }
    }

    private static void ViewAllClientes()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE TODOS LOS CLIENTES ---");
        try
        {
            var clientes = dbManager.GetClientes();
            if (clientes.Count == 0)
            {
                Console.WriteLine("No hay clientes registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} | {"Nombre",-20} | {"Apellido",-20} | {"Teléfono",-15} | {"Email",-30}");
                Console.WriteLine(new string('-', 95));
                foreach (var c in clientes)
                {
                    Console.WriteLine($"{c.Id,-5} | {c.Nombre,-20} | {c.Apellido,-20} | {c.Telefono,-15} | {c.Email,-30}");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de base de datos: {ex.Message}");
        }
    }

    private static void AddNuevoCliente()
    {
        Console.Clear();
        Console.WriteLine("--- AGREGAR NUEVO CLIENTE ---");
        string nombre = GetInput("Nombre: ");
        string apellido = GetInput("Apellido: ");
        string telefono = GetInput("Teléfono: ");
        string email = GetInput("Email (opcional): ");
        try
        {
            var nuevoCliente = new Cliente { Nombre = nombre, Apellido = apellido, Telefono = telefono, Email = string.IsNullOrWhiteSpace(email) ? null : email };
            dbManager.AddCliente(nuevoCliente);
            Console.WriteLine("\n¡Cliente agregado con éxito!");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error al agregar cliente: {ex.Message}");
        }
    }

    private static void UpdateCliente()
    {
        Console.Clear();
        Console.WriteLine("--- ACTUALIZAR CLIENTE ---");
        ViewAllClientes();
        Console.WriteLine("\nIngrese el ID del cliente que desea actualizar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var cliente = dbManager.GetClienteById(id);
            if (cliente == null)
            {
                Console.WriteLine("No se encontró un cliente con ese ID.");
                return;
            }

            Console.WriteLine($"\nCliente actual: ID: {cliente.Id} | Nombre: {cliente.Nombre} | Apellido: {cliente.Apellido} | Teléfono: {cliente.Telefono} | Email: {cliente.Email}");
            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para no cambiar):");

            string nuevoNombre = GetInput($"Nuevo Nombre ({cliente.Nombre}): ");
            if (!string.IsNullOrWhiteSpace(nuevoNombre)) cliente.Nombre = nuevoNombre;

            string nuevoApellido = GetInput($"Nuevo Apellido ({cliente.Apellido}): ");
            if (!string.IsNullOrWhiteSpace(nuevoApellido)) cliente.Apellido = nuevoApellido;

            string nuevoTelefono = GetInput($"Nuevo Teléfono ({cliente.Telefono}): ");
            if (!string.IsNullOrWhiteSpace(nuevoTelefono)) cliente.Telefono = nuevoTelefono;

            string nuevoEmail = GetInput($"Nuevo Email ({cliente.Email}): ");
            if (!string.IsNullOrWhiteSpace(nuevoEmail)) cliente.Email = nuevoEmail;

            try
            {
                dbManager.UpdateCliente(cliente);
                Console.WriteLine("\n¡Cliente actualizado con éxito!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }

    private static void DeleteCliente()
    {
        Console.Clear();
        Console.WriteLine("--- ELIMINAR CLIENTE ---");
        ViewAllClientes();
        Console.WriteLine("\nIngrese el ID del cliente que desea eliminar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (!dbManager.ClienteExists(id))
            {
                Console.WriteLine("\nEl cliente con ese ID no existe.");
                return;
            }
            try
            {
                dbManager.DeleteCliente(id);
                Console.WriteLine($"\nCliente con ID {id} eliminado con éxito.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }

    // --- Métodos de gestión de Vehículos ---
    private static void ManageVehiculos()
    {
        bool manage = true;
        while (manage)
        {
            Console.Clear();
            Console.WriteLine("--- MENÚ DE GESTIÓN DE VEHÍCULOS ---");
            Console.WriteLine("1. Ver vehículos de un cliente");
            Console.WriteLine("2. Agregar un nuevo vehículo");
            Console.WriteLine("3. Actualizar un vehículo existente");
            Console.WriteLine("4. Eliminar un vehículo");
            Console.WriteLine("5. Volver al menú principal");
            string option = GetInput("Seleccione una opción: ");
            switch (option)
            {
                case "1": ViewVehiculosDeCliente(); break;
                case "2": AddNuevoVehiculo(); break;
                case "3": UpdateVehiculo(); break;
                case "4": DeleteVehiculo(); break;
                case "5": manage = false; break;
                default: Console.WriteLine("Opción no válida. Intente de nuevo."); break;
            }
            if (manage)
            {
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey();
            }
        }
    }

    private static void ViewVehiculosDeCliente()
    {
        Console.Clear();
        Console.WriteLine("--- VER VEHÍCULOS DE UN CLIENTE ---");
        ViewAllClientes();
        Console.WriteLine("\nIngrese el ID del cliente para ver sus vehículos:");
        if (int.TryParse(Console.ReadLine(), out int clienteId))
        {
            if (!dbManager.ClienteExists(clienteId))
            {
                Console.WriteLine("El cliente no existe.");
                return;
            }
            Console.WriteLine(new string('-', 75));
            Console.WriteLine($"{"ID",-5} | {"Cliente ID",-12} | {"Marca",-15} | {"Modelo",-15} | {"Placa",-15}");
            Console.WriteLine(new string('-', 75));
            try
            {
                var vehiculos = dbManager.GetVehiculos(clienteId);
                if (vehiculos.Count == 0)
                {
                    Console.WriteLine($"No hay vehículos registrados para el cliente con ID {clienteId}.");
                }
                else
                {
                    foreach (var v in vehiculos)
                    {
                        Console.WriteLine($"{v.Id,-5} | {v.ClienteId,-12} | {v.Marca,-15} | {v.Modelo,-15} | {v.Placa,-15}");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de base de datos: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID de cliente inválido. Por favor, ingrese un número.");
        }
    }

    private static void AddNuevoVehiculo()
    {
        Console.Clear();
        Console.WriteLine("--- AGREGAR NUEVO VEHÍCULO ---");
        ViewAllClientes();
        Console.WriteLine("\nIngrese el ID del cliente al que pertenece este vehículo:");
        if (!int.TryParse(Console.ReadLine(), out int clienteId) || !dbManager.ClienteExists(clienteId))
        {
            Console.WriteLine("ID de cliente inválido o no existente.");
            return;
        }
        string marca = GetInput("Marca del vehículo: ");
        string modelo = GetInput("Modelo del vehículo: ");
        string placa = GetInput("Placa del vehículo: ");
        try
        {
            var nuevoVehiculo = new Vehiculo { ClienteId = clienteId, Marca = marca, Modelo = modelo, Placa = placa };
            dbManager.AddVehiculo(nuevoVehiculo);
            Console.WriteLine("\n¡Vehículo agregado con éxito!");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error al agregar vehículo: {ex.Message}");
        }
    }

    private static void UpdateVehiculo()
    {
        Console.Clear();
        Console.WriteLine("--- ACTUALIZAR VEHÍCULO ---");
        Console.WriteLine("Ingrese el ID del vehículo que desea actualizar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var vehiculo = dbManager.GetVehiculoById(id);
            if (vehiculo == null)
            {
                Console.WriteLine("No se encontró un vehículo con ese ID.");
                return;
            }

            Console.WriteLine($"\nVehículo actual: ID: {vehiculo.Id} | Marca: {vehiculo.Marca} | Modelo: {vehiculo.Modelo} | Placa: {vehiculo.Placa}");
            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para no cambiar):");

            string nuevaMarca = GetInput($"Nueva Marca ({vehiculo.Marca}): ");
            if (!string.IsNullOrWhiteSpace(nuevaMarca)) vehiculo.Marca = nuevaMarca;

            string nuevoModelo = GetInput($"Nuevo Modelo ({vehiculo.Modelo}): ");
            if (!string.IsNullOrWhiteSpace(nuevoModelo)) vehiculo.Modelo = nuevoModelo;

            string nuevaPlaca = GetInput($"Nueva Placa ({vehiculo.Placa}): ");
            if (!string.IsNullOrWhiteSpace(nuevaPlaca)) vehiculo.Placa = nuevaPlaca;

            try
            {
                dbManager.UpdateVehiculo(vehiculo);
                Console.WriteLine("\n¡Vehículo actualizado con éxito!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar vehículo: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }

    private static void DeleteVehiculo()
    {
        Console.Clear();
        Console.WriteLine("--- ELIMINAR VEHÍCULO ---");
        Console.WriteLine("Ingrese el ID del vehículo que desea eliminar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (!dbManager.VehiculoExists(id))
            {
                Console.WriteLine("\nEl vehículo con ese ID no existe.");
                return;
            }
            try
            {
                dbManager.DeleteVehiculo(id);
                Console.WriteLine($"\nVehículo con ID {id} eliminado con éxito.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar vehículo: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }

    // --- Métodos de gestión de Empleados ---
    private static void ManageEmpleados()
    {
        bool manage = true;
        while (manage)
        {
            Console.Clear();
            Console.WriteLine("--- MENÚ DE GESTIÓN DE EMPLEADOS ---");
            Console.WriteLine("1. Ver todos los empleados");
            Console.WriteLine("2. Agregar un nuevo empleado");
            Console.WriteLine("3. Actualizar un empleado existente");
            Console.WriteLine("4. Eliminar un empleado");
            Console.WriteLine("5. Volver al menú principal");
            string option = GetInput("Seleccione una opción: ");
            switch (option)
            {
                case "1": ViewAllEmpleados(); break;
                case "2": AddNuevoEmpleado(); break;
                case "3": UpdateEmpleado(); break;
                case "4": DeleteEmpleado(); break;
                case "5": manage = false; break;
                default: Console.WriteLine("Opción no válida. Intente de nuevo."); break;
            }
            if (manage)
            {
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey();
            }
        }
    }

    private static void ViewAllEmpleados()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE TODOS LOS EMPLEADOS ---");
        try
        {
            var empleados = dbManager.GetEmpleados();
            if (empleados.Count == 0)
            {
                Console.WriteLine("No hay empleados registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} | {"Nombre",-20} | {"Apellido",-20} | {"Puesto",-20} | {"Salario",-15}");
                Console.WriteLine(new string('-', 85));
                foreach (var e in empleados)
                {
                    Console.WriteLine($"{e.Id,-5} | {e.Nombre,-20} | {e.Apellido,-20} | {e.Puesto,-20} | {e.Salario,-15:C}");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de base de datos: {ex.Message}");
        }
    }

    private static void AddNuevoEmpleado()
    {
        Console.Clear();
        Console.WriteLine("--- AGREGAR NUEVO EMPLEADO ---");
        string nombre = GetInput("Nombre: ");
        string apellido = GetInput("Apellido: ");
        string puesto = GetInput("Puesto: ");
        decimal salario;
        while (!decimal.TryParse(GetInput("Salario: "), out salario))
        {
            Console.WriteLine("Salario inválido. Por favor, ingrese un valor numérico.");
        }
        try
        {
            var nuevoEmpleado = new Empleado { Nombre = nombre, Apellido = apellido, Puesto = puesto, Salario = salario };
            dbManager.AddEmpleado(nuevoEmpleado);
            Console.WriteLine("\n¡Empleado agregado con éxito!");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error al agregar empleado: {ex.Message}");
        }
    }

    private static void UpdateEmpleado()
    {
        Console.Clear();
        Console.WriteLine("--- ACTUALIZAR EMPLEADO ---");
        ViewAllEmpleados();
        Console.WriteLine("\nIngrese el ID del empleado que desea actualizar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var empleado = dbManager.GetEmpleadoById(id);
            if (empleado == null)
            {
                Console.WriteLine("No se encontró un empleado con ese ID.");
                return;
            }

            Console.WriteLine($"\nEmpleado actual: ID: {empleado.Id} | Nombre: {empleado.Nombre} | Apellido: {empleado.Apellido} | Puesto: {empleado.Puesto} | Salario: {empleado.Salario:C}");
            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para no cambiar):");

            string nuevoNombre = GetInput($"Nuevo Nombre ({empleado.Nombre}): ");
            if (!string.IsNullOrWhiteSpace(nuevoNombre)) empleado.Nombre = nuevoNombre;

            string nuevoApellido = GetInput($"Nuevo Apellido ({empleado.Apellido}): ");
            if (!string.IsNullOrWhiteSpace(nuevoApellido)) empleado.Apellido = nuevoApellido;

            string nuevoPuesto = GetInput($"Nuevo Puesto ({empleado.Puesto}): ");
            if (!string.IsNullOrWhiteSpace(nuevoPuesto)) empleado.Puesto = nuevoPuesto;

            string nuevoSalarioStr = GetInput($"Nuevo Salario ({empleado.Salario:C}): ");
            if (!string.IsNullOrWhiteSpace(nuevoSalarioStr))
            {
                if (decimal.TryParse(nuevoSalarioStr, out decimal nuevoSalario))
                {
                    empleado.Salario = nuevoSalario;
                }
                else
                {
                    Console.WriteLine("Salario inválido. No se actualizó el salario.");
                }
            }

            try
            {
                dbManager.UpdateEmpleado(empleado);
                Console.WriteLine("\n¡Empleado actualizado con éxito!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar empleado: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }

    private static void DeleteEmpleado()
    {
        Console.Clear();
        Console.WriteLine("--- ELIMINAR EMPLEADO ---");
        ViewAllEmpleados();
        Console.WriteLine("\nIngrese el ID del empleado que desea eliminar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (!dbManager.EmpleadoExists(id))
            {
                Console.WriteLine("\nEl empleado con ese ID no existe.");
                return;
            }
            try
            {
                dbManager.DeleteEmpleado(id);
                Console.WriteLine($"\nEmpleado con ID {id} eliminado con éxito.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }

    // --- Métodos de gestión de Tipos de Lavado ---
    private static void ManageTiposLavado()
    {
        bool manage = true;
        while (manage)
        {
            Console.Clear();
            Console.WriteLine("--- MENÚ DE GESTIÓN DE SERVICIOS ---");
            Console.WriteLine("1. Ver todos los servicios");
            Console.WriteLine("2. Agregar un nuevo servicio");
            Console.WriteLine("3. Actualizar un servicio existente");
            Console.WriteLine("4. Eliminar un servicio");
            Console.WriteLine("5. Volver al menú principal");
            string option = GetInput("Seleccione una opción: ");
            switch (option)
            {
                case "1": ViewAllTiposLavado(); break;
                case "2": AddNuevoTipoLavado(); break;
                case "3": UpdateTipoLavado(); break;
                case "4": DeleteTipoLavado(); break;
                case "5": manage = false; break;
                default: Console.WriteLine("Opción no válida. Intente de nuevo."); break;
            }
            if (manage)
            {
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey();
            }
        }
    }

    private static void ViewAllTiposLavado()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE SERVICIOS ---");
        try
        {
            var tipos = dbManager.GetTiposLavado();
            if (tipos.Count == 0)
            {
                Console.WriteLine("No hay servicios registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} | {"Servicio",-30} | {"Precio",-15}");
                Console.WriteLine(new string('-', 55));
                foreach (var t in tipos)
                {
                    Console.WriteLine($"{t.Id,-5} | {t.NombreServicio,-30} | {t.Precio,-15:C}");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error de base de datos: {ex.Message}");
        }
    }

    private static void AddNuevoTipoLavado()
    {
        Console.Clear();
        Console.WriteLine("--- AGREGAR NUEVO SERVICIO ---");
        string nombreServicio = GetInput("Nombre del servicio: ");
        string descripcion = GetInput("Descripción del servicio (opcional): ");
        decimal precio;
        while (!decimal.TryParse(GetInput("Precio: "), out precio))
        {
            Console.WriteLine("Precio inválido. Por favor, ingrese un valor numérico.");
        }
        try
        {
            var nuevoTipo = new TipoLavado { NombreServicio = nombreServicio, Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion, Precio = precio };
            dbManager.AddTipoLavado(nuevoTipo);
            Console.WriteLine("\n¡Servicio agregado con éxito!");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error al agregar servicio: {ex.Message}");
        }
    }

    private static void UpdateTipoLavado()
    {
        Console.Clear();
        Console.WriteLine("--- ACTUALIZAR SERVICIO ---");
        ViewAllTiposLavado();
        Console.WriteLine("\nIngrese el ID del servicio que desea actualizar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tipo = dbManager.GetTipoLavadoById(id);
            if (tipo == null)
            {
                Console.WriteLine("No se encontró un servicio con ese ID.");
                return;
            }

            Console.WriteLine($"\nServicio actual: ID: {tipo.Id} | Nombre: {tipo.NombreServicio} | Precio: {tipo.Precio:C}");
            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para no cambiar):");

            string nuevoNombre = GetInput($"Nuevo Nombre ({tipo.NombreServicio}): ");
            if (!string.IsNullOrWhiteSpace(nuevoNombre)) tipo.NombreServicio = nuevoNombre;

            string nuevoPrecioStr = GetInput($"Nuevo Precio ({tipo.Precio:C}): ");
            if (!string.IsNullOrWhiteSpace(nuevoPrecioStr))
            {
                if (decimal.TryParse(nuevoPrecioStr, out decimal nuevoPrecio))
                {
                    tipo.Precio = nuevoPrecio;
                }
                else
                {
                    Console.WriteLine("Precio inválido. No se actualizó el precio.");
                }
            }

            try
            {
                dbManager.UpdateTipoLavado(tipo);
                Console.WriteLine("\n¡Servicio actualizado con éxito!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar servicio: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }

    private static void DeleteTipoLavado()
    {
        Console.Clear();
        Console.WriteLine("--- ELIMINAR SERVICIO ---");
        ViewAllTiposLavado();
        Console.WriteLine("\nIngrese el ID del servicio que desea eliminar:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (!dbManager.TipoLavadoExists(id))
            {
                Console.WriteLine("\nEl servicio con ese ID no existe.");
                return;
            }
            try
            {
                dbManager.DeleteTipoLavado(id);
                Console.WriteLine($"\nServicio con ID {id} eliminado con éxito.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar servicio: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nID inválido. Por favor, ingrese un número.");
        }
    }
}