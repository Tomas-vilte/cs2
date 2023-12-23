using Swed64;
using System.Runtime.InteropServices;


// Creamos una clase llamada JumpController para encapsular la lógica de control de salto
public class JumpController
{
    private readonly Swed _swed;
    private readonly IntPtr _client;
    private readonly IntPtr _forceJumpAddress;

    // Definimos constantes para las teclas y estados
    private const int SPACE_BAR = 0x20;
    private const uint STANDING = 65665;
    private const uint CROUCHING = 65667;
    private const uint PLUS_JUMP = 65537; // +jump
    private const uint MINUS_JUMP = 256; // -jump

    // Importamos la función GetAsyncKeyState del API de Windows para comprobar el estado de las teclas
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    // Constructor de la clase JumpController
    public JumpController()
    {
        _swed = new Swed("cs2");
        _client = _swed.GetModuleBase("client.dll");
        _forceJumpAddress = _client + 0x16C2380;

    }

    // Metodo para iniciar el control de salto
    public void StartJumpControl()
    {
        // Bucle infinito para controlar el salto
        while (true)
        {
            // Obtiene la dirección de memoria del jugador
            IntPtr playerPawnAddress = _swed.ReadPointer(_client, 0x16C8F38);
            uint fFlag = _swed.ReadUInt(playerPawnAddress, 0x3C8);

            // Si la tecla espacio está presionada
            if (GetAsyncKeyState(SPACE_BAR) < 0)
            {
                // Si el jugador está parado o agachado
                if (fFlag == STANDING || fFlag == CROUCHING)
                {
                    Thread.Sleep(1);
                    // Escribe el valor PLUS_JUMP en la memoria
                    _swed.WriteUInt(_forceJumpAddress, PLUS_JUMP);
                }
                // Si el jugador está en el aire
                else
                {
                    // Escribe el valor MINUS_JUMP en la memoria
                    _swed.WriteUInt(_forceJumpAddress, MINUS_JUMP);
                }
            }
            // Pausamos el hilo 5s
            Thread.Sleep(5);
        }
    }
}
