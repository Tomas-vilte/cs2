
namespace Counter_Strike_2_Multi
{
    public class Offsets
    {

        // Bases
        public int localPlayer = 0x16C8F38;
        public int entityList = 0x16D5C60;
        public int viewmatrix = 0x1820150; //0x1820830

        // Atributos
        public int teamNum = 0x3bf;
        public int jumpFlag = 0x3c8;
        public int health = 0x32C;
        public int origin = 0xCD8;

    }

    public static class Player
    {
        // offsests.cs
        public static int dwLocalPlayerPawn = 0x16C8F38;
        public static int dwEntityList = 0x17C1950;
        public static int dwViewMatrix = 0x1820150;

        public static int m_hPlayerPawn = 0x7EC;
        public static int m_vOldOrigin = 0x1224;
        public static int m_iTeamNum = 0x3BF;
        public static int m_lifeState = 0x330;
        public static int m_modelState = 0x160;
        public static int m_pGameSceneNode = 0x310;
    }
}