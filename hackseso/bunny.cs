using Swed64;
using System.Formats.Asn1;
using System.Runtime.InteropServices;

// Inicializa la clase swed
Swed swed = new Swed("cs2");

// Creamos las constantes, donde esta los valores del espacio
const int SPACE_BAR = 0x20;

// Agachado y salto
const uint STANDING = 65665;
const uint CROUCHING = 65667;

// +jump y -jump
const uint PLUS_JUMP = 65537; // +jump
const uint MINUS_JUMP = 256; // -jump

// modulo "client.dll"
IntPtr client = swed.GetModuleBase("client.dll");

// Calcula la dirección de memoria para el valor "forceJump"
IntPtr forceJumpAddress = client + 0x16C2380;

// Bucle infinito para controlar el salto
while (true)
{
    // Obtiene la dirección de memoria del jugador
    IntPtr playerPawnAddress = swed.ReadPointer(client, 0x16C8F38);

    // Obtiene el estado actual del jugador
    uint fFlag = swed.ReadUInt(playerPawnAddress, 0x3C8);

    // Si la tecla espacio está presionada
    if (GetAsyncKeyState(SPACE_BAR) < 0)
    {
        // Si el jugador está parado o agachado
        if (fFlag == STANDING || fFlag == CROUCHING)
        {
            // Pausa el hilo durante 1 milisegundo
            Thread.Sleep(1);

            // Escribe el valor "PLUS_JUMP" en la memoria
            swed.WriteUInt(forceJumpAddress, PLUS_JUMP);
        }
        // Si el jugador está en el aire
        else
        {
            // Escribe el valor "MINUS_JUMP" en la memoria
            swed.WriteUInt(forceJumpAddress, MINUS_JUMP);
        }
    }

    // Pausa el hilo durante 5 milisegundos
    Thread.Sleep(5);
}

// Importa la función "GetAsyncKeyState" del API de Windows para comprobar el estado de las teclas
[DllImport("user32.dll")]

static extern short GetAsyncKeyState(int vKey);
