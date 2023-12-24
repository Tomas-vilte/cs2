using Swed64;
using System.Numerics;


namespace hackseso
{
    public class Render
    {
        Swed swed;

        public Reader(Swed swed) { this.swed = swed; }

        public List<Vector3> ReadBones(IntPtr boneAddress)
        {
            byte[] boneBytes = swed.ReadBytes(boneAddress, 27 * 32 + 16);
            List<Vector3> bones = new List<Vector3>();

            foreach(var boneId in Enum.GetValues(typeof(BoneIds)))
            {
                float x = BitConverter.ToSingle(boneBytes, (int)boneId * 32 + 0);
                float y = BitConverter.ToSingle(boneBytes, (int)boneId * 32 + 4);
                float z = BitConverter.ToSingle(boneBytes, (int)boneId * 32 + 8);
                Vector3 currentBone = new Vector3(x, y, z);
                bones.Add(currentBone);
            }
            return bones;
           
        }
        public List<Vector2> ReadBones2d(List<Vector3> bones,  ViewMatrix viewMatrix, Vector2 screenSize)
        {
            List<Vector2> bones2d = new List<Vector2>();

        }
    }
}
