using ImGuiNET;
using System.Numerics;
using System.Windows.Input;
using ClickableTransparentOverlay;
using Veldrid;

namespace hackseso
{
    public class Renderer : Overlay
    {
        public Vector2 overlaySize = new Vector2(1920, 1080);
        Vector2 windowLocation = new Vector2(0, 0);

        public List<Entity> entitiesCopy = new List<Entity>();

        public Entity LocalPlayerCopy = new Entity();
        ImDrawListPtr drawList;

        public bool esp = true;
        Vector4 teamColor = new Vector4(1, 1, 1, 1);
        Vector4 enemyColor = new Vector4(1, 1, 1, 1);

        float boneThickness = 4;
        bool showOverlay = true;

        protected override void Render()
        {
            
            if (Console.KeyAvailable)
            {
              
                ConsoleKeyInfo key = Console.ReadKey(true);

                
                if (key.Key == ConsoleKey.F1)
                {
                    showOverlay = !showOverlay;
                }
            }

            if (showOverlay)
            {
                ImGui.Begin("Bone esp");
                ImGui.Checkbox("esp", ref esp);
                ImGui.SliderFloat("bone thickness", ref boneThickness, 4, 500);

                if (ImGui.CollapsingHeader("team color"))
                    ImGui.ColorPicker4("##teamcolor", ref teamColor);

                if (ImGui.CollapsingHeader("enemy color"))
                    ImGui.ColorPicker4("##enemycolor", ref enemyColor);

                if (esp)
                {
                    DrawOverlay();
                    DrawSkeletons();
                }
                ImGui.End();
            }
        }


        void DrawSkeletons()
            {
                if (entitiesCopy.Count == 0 || entitiesCopy == null)
                    return;

                List<Entity> tempEntities = new List<Entity>(entitiesCopy).ToList();

                drawList = ImGui.GetWindowDrawList();
                uint uintColor;

                foreach (Entity entity in tempEntities)
                {
                    if (entity == null) continue;
                    uintColor = LocalPlayerCopy.team == entity.team ? ImGui.ColorConvertFloat4ToU32(teamColor) : ImGui.ColorConvertFloat4ToU32(enemyColor);

                    if (entity.bones2d[2].X > 0 && entity.bones2d[2].Y > 0 && entity.bones2d[2].X < overlaySize.X && entity.bones2d[2].Y < overlaySize.Y)
                    {
                        float currentBoneThickNess = boneThickness / entity.distance;

                        drawList.AddLine(entity.bones2d[1], entity.bones2d[2], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[1], entity.bones2d[3], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[1], entity.bones2d[6], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[3], entity.bones2d[4], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[6], entity.bones2d[7], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[4], entity.bones2d[5], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[7], entity.bones2d[8], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[1], entity.bones2d[0], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[0], entity.bones2d[9], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[0], entity.bones2d[11], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[9], entity.bones2d[10], uintColor, currentBoneThickNess);
                        drawList.AddLine(entity.bones2d[11], entity.bones2d[12], uintColor, currentBoneThickNess);
                        drawList.AddCircle(entity.bones2d[2], 3 + currentBoneThickNess, uintColor);

                    }
                }
            }


            void DrawOverlay()
            {
                ImGui.SetNextWindowSize(overlaySize);
                ImGui.SetNextWindowPos(windowLocation);
                ImGui.Begin("overlay", ImGuiWindowFlags.NoDecoration
                    | ImGuiWindowFlags.NoBackground
                    | ImGuiWindowFlags.NoBringToFrontOnFocus
                    | ImGuiWindowFlags.NoMove
                    | ImGuiWindowFlags.NoInputs
                    | ImGuiWindowFlags.NoCollapse
                    | ImGuiWindowFlags.NoScrollbar
                    | ImGuiWindowFlags.NoScrollWithMouse);
            }
        }
    }