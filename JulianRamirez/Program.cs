using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulianRamirez
{
    internal class Program
    {
        // El ultimo digito del carne es 7.
        // N = 7 + 10 = 17 registros maximos.
        const int N = 17;

        // Vectores sincronizados.
        // Cada paciente y su gravedad utilizan la misma posicion.
        static string[] pacientes = new string[N];
        static int[] gravedad = new int[N];

        // Cantidad de pacientes registrados actualmente.
        static int cantidadPacientes = 0;

        static void Main(string[] args)
        {
            int opcion = 0;

            // Inicializamos los vectores.
            Inicializar();

            // Menu principal.
            while (opcion != 5)
            {
                Console.Clear();

                Console.WriteLine("======================================");
                Console.WriteLine("        HOSPITAL SAN RAFAEL");
                Console.WriteLine("   MONITOREO DE SIGNOS Y GRAVEDAD");
                Console.WriteLine("======================================");
                Console.WriteLine();

                Console.WriteLine("1. Ingresar paciente");
                Console.WriteLine("2. Consultar pacientes");
                Console.WriteLine("3. Modificar gravedad");
                Console.WriteLine("4. Eliminar paciente");
                Console.WriteLine("5. Salir");

                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            IngresarPaciente();
                            break;

                        case 2:
                            ConsultarPacientes();
                            break;

                        case 3:
                            ModificarGravedad();
                            break;

                        case 4:
                            EliminarPaciente();
                            break;

                        case 5:
                            Console.WriteLine();
                            Console.WriteLine("Saliendo del sistema...");
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("Opcion invalida.");
                            Pausa();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Debe ingresar un numero.");
                    Pausa();
                    opcion = 0;
                }
            }
        }

        // Inicializa los 17 espacios de los vectores.
        static void Inicializar()
        {
            for (int i = 0; i < N; i++)
            {
                pacientes[i] = "";
                gravedad[i] = 0;
            }
        }

        // Permite ingresar un nuevo paciente.
        static void IngresarPaciente()
        {
            Console.Clear();

            Console.WriteLine("=== INGRESAR PACIENTE ===");
            Console.WriteLine();

            // Verifica que no se supere el limite de 17 pacientes.
            if (cantidadPacientes >= N)
            {
                Console.WriteLine("No hay espacio para mas pacientes.");
                Console.WriteLine("Se alcanzo el limite maximo de 17 registros.");
                Pausa();
                return;
            }

            string nombre;

            do
            {
                Console.Write("Nombre del paciente: ");
                nombre = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    Console.WriteLine("El nombre no puede estar vacio.");
                }

            } while (string.IsNullOrWhiteSpace(nombre));

            int nivel = LeerGravedad();

            // Nombre y gravedad se guardan en la misma posicion.
            pacientes[cantidadPacientes] = nombre;
            gravedad[cantidadPacientes] = nivel;

            cantidadPacientes++;

            Console.WriteLine();
            Console.WriteLine("Paciente registrado correctamente.");

            Pausa();
        }

        // Muestra todos los pacientes registrados.
        static void ConsultarPacientes()
        {
            Console.Clear();

            Console.WriteLine("=== LISTA DE PACIENTES ===");
            Console.WriteLine();

            if (cantidadPacientes == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
            }
            else
            {
                Console.WriteLine("No.   Paciente                 Gravedad");
                Console.WriteLine("----------------------------------------");

                for (int i = 0; i < cantidadPacientes; i++)
                {
                    Console.WriteLine(
                        (i + 1) + ".    " +
                        pacientes[i].PadRight(24) +
                        gravedad[i]
                    );
                }

                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Total de pacientes: " + cantidadPacientes);
            }

            Pausa();
        }

        // Permite cambiar el nivel de gravedad de un paciente.
        static void ModificarGravedad()
        {
            Console.Clear();

            Console.WriteLine("=== MODIFICAR GRAVEDAD ===");
            Console.WriteLine();

            if (cantidadPacientes == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                Pausa();
                return;
            }

            MostrarPacientes();

            Console.WriteLine();
            Console.Write("Digite el nombre del paciente: ");
            string nombreBuscado = Console.ReadLine();

            int indice = BuscarPaciente(nombreBuscado);

            if (indice != -1)
            {
                Console.WriteLine();
                Console.WriteLine("Paciente encontrado: " + pacientes[indice]);
                Console.WriteLine("Gravedad actual: " + gravedad[indice]);
                Console.WriteLine();

                int nuevoNivel = LeerGravedad();

                // Se modifica la gravedad utilizando el mismo indice.
                gravedad[indice] = nuevoNivel;

                Console.WriteLine();
                Console.WriteLine("Gravedad actualizada correctamente.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Paciente no encontrado.");
            }

            Pausa();
        }

        // Elimina un paciente manteniendo los vectores sincronizados.
        static void EliminarPaciente()
        {
            Console.Clear();

            Console.WriteLine("=== ELIMINAR PACIENTE ===");
            Console.WriteLine();

            if (cantidadPacientes == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                Pausa();
                return;
            }

            MostrarPacientes();

            Console.WriteLine();
            Console.Write("Digite el nombre del paciente a eliminar: ");
            string nombreBuscado = Console.ReadLine();

            int indice = BuscarPaciente(nombreBuscado);

            if (indice != -1)
            {
                string pacienteEliminado = pacientes[indice];

                // Desplazamos ambos vectores al mismo tiempo.
                // Esto evita perder la sincronizacion de los datos.
                for (int i = indice; i < cantidadPacientes - 1; i++)
                {
                    pacientes[i] = pacientes[i + 1];
                    gravedad[i] = gravedad[i + 1];
                }

                // Limpiamos la ultima posicion que estaba ocupada.
                pacientes[cantidadPacientes - 1] = "";
                gravedad[cantidadPacientes - 1] = 0;

                cantidadPacientes--;

                Console.WriteLine();
                Console.WriteLine("Paciente " + pacienteEliminado +
                                  " eliminado correctamente.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Paciente no encontrado.");
            }

            Pausa();
        }

        // Busca un paciente por su nombre.
        // Si lo encuentra devuelve su posicion.
        // Si no lo encuentra devuelve -1.
        static int BuscarPaciente(string nombreBuscado)
        {
            for (int i = 0; i < cantidadPacientes; i++)
            {
                if (pacientes[i].Equals(nombreBuscado,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        // Solicita y valida que la gravedad este entre 1 y 5.
        static int LeerGravedad()
        {
            int nivel;
            bool valido = false;

            do
            {
                Console.Write("Nivel de gravedad (1 a 5): ");

                if (int.TryParse(Console.ReadLine(), out nivel))
                {
                    if (nivel >= 1 && nivel <= 5)
                    {
                        valido = true;
                    }
                    else
                    {
                        Console.WriteLine(
                            "La gravedad debe estar entre 1 y 5."
                        );
                    }
                }
                else
                {
                    Console.WriteLine(
                        "Debe ingresar un numero entero."
                    );
                }

            } while (!valido);

            return nivel;
        }

        // Muestra los pacientes antes de modificar o eliminar.
        static void MostrarPacientes()
        {
            Console.WriteLine("Pacientes registrados:");
            Console.WriteLine();

            for (int i = 0; i < cantidadPacientes; i++)
            {
                Console.WriteLine(
                    (i + 1) + ". " +
                    pacientes[i] +
                    " - Gravedad: " +
                    gravedad[i]
                );
            }
        }

        // Pausa el programa para poder leer los resultados.
        static void Pausa()
        {
            Console.WriteLine();
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}