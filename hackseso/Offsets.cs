
namespace hackseso
{
    public class Offsets
    {

        // Bases    
        public int localPlayer = 0x1729348;
        public int entityList = 0x17CE6A0;
        public int viewmatrix = 0x1820EA0; //0x1820830

        // Atributos
        public int teamNum = 0x3bf;
        public int jumpFlag = 0x3c8;
        public int health = 0x32C;
        public int origin = 0xCD8;

    }

    public static class Player
    {
        // offsests.cs
        public static int origin = 0xCD8;
        public static int dwLocalPlayerPawn = 0x1729348;
        public static int dwEntityList = 0x18B3FA8;
        public static int dwViewMatrix = 0x19154C0;
        public static int m_iIDEntIndex = 0x15A4;
        
        public static int m_hPlayerPawn = 0x7EC;
        public static int m_vOldOrigin = 0x1224;
        public static int m_iTeamNum = 0x3CB;
        public static int m_lifeState = 0x330;
        public static int m_modelState = 0x160;
        public static int m_pGameSceneNode = 0x310;
        public static int m_flFlashBangTime = 0x145C;
        public static int m_flDetectedByEnemySensorTime = 0x13E4;
    }
}

