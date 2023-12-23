using Swed64;
using System.Formats.Asn1;
using System.Runtime.InteropServices;

// Inicializamos la clase swed
Swed swed = new Swed("cs2");

// Creamos las constantes, donde esta los valores de la tecla
const int SPACE_BAR = 0x20;

// Agachado y salto
const uint STANDING = 65665;
const uint CROUCHING = 65667;

// +jump y -jump
const uint PLUS_JUMP = 65537; // +jump
const uint MINUS_JUMP = 256; // -jump

IntPtr client = swed.GetModuleBase("client.dll");
IntPtr forceJumpAddress = client + 0x16C2380;

while (true)
{
    IntPtr playerPawnAddress = swed.ReadPointer(client, 0x16C8F38);
    uint fFlag = swed.ReadUInt(playerPawnAddress, 0x3C8);

    if (GetAsyncKeyState(SPACE_BAR) < 0)
    {
        if (fFlag == STANDING || fFlag == CROUCHING) // si estamos agachados
        {
            Thread.Sleep(1); // no me dejaría saltar sin él

            swed.WriteUInt(forceJumpAddress, PLUS_JUMP); // +jump
        }
        else // si estamos en el aire
        {
            swed.WriteUInt(forceJumpAddress, MINUS_JUMP); // -jump
        }
    }
    Thread.Sleep(5); // reiniciamos el thread
}

[DllImport("user32.dll")]

static extern short GetAsyncKeyState(int vKey);