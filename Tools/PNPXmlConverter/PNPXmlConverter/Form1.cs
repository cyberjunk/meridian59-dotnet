using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.Globalization;
using Mogre;
using System.Threading;

namespace PNPXmlConverter
{
    public partial class Form1 : Form
    {
        private XmlReader reader;
        private XmlWriter writer;

        private NumberFormatInfo NumberFormatInfo;

        private float scale, x, z, worldsize;
        
        public Form1()
        {
            InitializeComponent();

            NumberFormatInfo = new NumberFormatInfo();
            NumberFormatInfo.NumberDecimalSeparator = ".";

            // make sure to always use "." in float notation instead of ","
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        }

        private void btnSelectGrass_Click(object sender, EventArgs e)
        {
            openFileDialogGrass.ShowDialog();
        }

        private void btnSelectTrees_Click(object sender, EventArgs e)
        {
            openFileDialogTrees.ShowDialog();
        }

        private void openFileDialogGrass_FileOk(object sender, CancelEventArgs e)
        {
            txtGrassFilename.Text = openFileDialogGrass.FileName;
        }

        private void openFileDialogTrees_FileOk(object sender, CancelEventArgs e)
        {
            txtTreeFilename.Text = openFileDialogTrees.FileName;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            scale = float.Parse(txtScale.Text, NumberFormatInfo);
            worldsize = float.Parse(txtWorldSize.Text, NumberFormatInfo);
            x = float.Parse(txtX.Text, NumberFormatInfo);
            z = float.Parse(txtZ.Text, NumberFormatInfo);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            Log("Creating nodes.xml");
            writer = XmlWriter.Create("nodes.xml", settings);

            // write
            writer.WriteStartDocument();
            writer.WriteStartElement("nodes");
            writer.WriteAttributeString("count", "0");

            // process grass   
            Log("Reading " + txtGrassFilename.Text);
            reader = XmlReader.Create(txtGrassFilename.Text);
            int grassnodes = ConvertGrass(reader, writer);
            reader.Close();
            Log("Exported " + grassnodes + " grassnodes.");

            // process trees
            Log("Reading " + txtTreeFilename.Text);
            reader = XmlReader.Create(txtTreeFilename.Text);
            int treenodes = ConvertTrees(reader, writer);
            reader.Close();
            Log("Exported " + treenodes + " treenodes.");
                       
            // end
            writer.WriteEndElement();
            writer.WriteEndDocument();          
            writer.Close();
            
            int sum = grassnodes + treenodes;
            Log("Sum: " + sum.ToString() + " nodes.");

            // replace count
            Log("Replacing count in nodes.xml");
            string s = System.IO.File.ReadAllText("nodes.xml");
            System.IO.File.WriteAllText("nodes.xml", s.Replace("count=\"0\"", "count=\"" + sum + "\""));

            Log("Export finished.");
        }

        private int ConvertGrass(XmlReader reader, XmlWriter writer)
        {
            int count = 0;
            while (reader.ReadToFollowing("GRASSPLACEMENT"))
            {
                string meshname = reader["NAME"].Replace(".x", ".mesh");

                writer.WriteStartElement("node");
                writer.WriteAttributeString("name", "grass_" + count.ToString());
                writer.WriteAttributeString("collisions", false.ToString());

                // POSITION
                reader.ReadToFollowing("POSITION");
                writer.WriteStartElement("position");
                // PNP Z-axis is Ogre Y-axis! so Y is read from Z and vice versa
                float newx = (float.Parse(reader["X"], NumberFormatInfo) * scale) + x; // add our offset
                float newy = (float.Parse(reader["Z"], NumberFormatInfo) * scale);
                float newz = (worldsize / 2.0f) + ((worldsize / 2.0f) - (float.Parse(reader["Y"], NumberFormatInfo) * scale)) + z; // add our offset

                writer.WriteAttributeString("x", newx.ToString());
                writer.WriteAttributeString("y", newy.ToString());
                writer.WriteAttributeString("z", newz.ToString());
                writer.WriteEndElement();

                // ORIENTATION
                reader.ReadToFollowing("ORIENTATION");
                writer.WriteStartElement("orientation");

                string strx = reader["X"];
                string stry = reader["Y"];
                string strz = reader["Z"];
                float angleX = 0.0f;
                float angleY = 0.0f;
                float angleZ = 0.0f;

                if (strx != null) angleX = float.Parse(strx, NumberFormatInfo);
                if (stry != null) angleY = float.Parse(stry, NumberFormatInfo);
                if (strz != null) angleZ = float.Parse(strz, NumberFormatInfo);

                // PNP Z-axis is Ogre Y-axis! So 1. argument is Z-angle
                Quaternion quat = EulerAnglesToQuaternion(new Degree(angleZ), new Degree(angleX), new Degree(angleY));

                writer.WriteAttributeString("qw", quat.w.ToString());
                writer.WriteAttributeString("qx", quat.x.ToString());
                writer.WriteAttributeString("qy", quat.y.ToString());
                writer.WriteAttributeString("qz", quat.z.ToString());
                writer.WriteEndElement();

                // SIZE
                reader.ReadToFollowing("SIZE");
                writer.WriteStartElement("scale");
                float subscale = float.Parse(reader["SCALE"], NumberFormatInfo);
                writer.WriteAttributeString("x", subscale.ToString());
                writer.WriteAttributeString("y", subscale.ToString());
                writer.WriteAttributeString("z", subscale.ToString());
                writer.WriteEndElement();

                // ENTITY
                writer.WriteStartElement("entity");
                writer.WriteAttributeString("meshfile", meshname);
                writer.WriteAttributeString("castshadows", chkGrassShadows.Checked.ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
                count++;
            }

            return count;
        }

        private int ConvertTrees(XmlReader reader, XmlWriter writer)
        {
            int count = 0;
            while (reader.ReadToFollowing("TREEPLACEMENT"))
            {
                string meshname = reader["NAME"].Replace(".x", ".mesh");

                writer.WriteStartElement("node");
                writer.WriteAttributeString("name", "tree_" + count.ToString());
                writer.WriteAttributeString("collisions", false.ToString());

                // POSITION
                reader.ReadToFollowing("POSITION");
                writer.WriteStartElement("position");
                // PNP Z-axis is Ogre Y-axis! so Y is read from Z and vice versa
                float newx = (float.Parse(reader["X"], NumberFormatInfo) * scale) + x; // add our offset
                float newy = (float.Parse(reader["Z"], NumberFormatInfo) * scale);
                float newz = (worldsize / 2.0f) + ((worldsize / 2.0f) - (float.Parse(reader["Y"], NumberFormatInfo) * scale)) + z; // add our offset

                writer.WriteAttributeString("x", newx.ToString());
                writer.WriteAttributeString("y", newy.ToString());
                writer.WriteAttributeString("z", newz.ToString());
                writer.WriteEndElement();

                // ORIENTATION
                reader.ReadToFollowing("ORIENTATION");
                writer.WriteStartElement("orientation");

                string strx = reader["X"];
                string stry = reader["Y"];
                string strz = reader["Z"];
                float angleX = 0.0f;
                float angleY = 0.0f;
                float angleZ = 0.0f;

                if (strx != null) angleX = float.Parse(strx, NumberFormatInfo);
                if (stry != null) angleY = float.Parse(stry, NumberFormatInfo);
                if (strz != null) angleZ = float.Parse(strz, NumberFormatInfo);

                // PNP Z-axis is Ogre Y-axis! So 1. argument is Z-angle
                Quaternion quat = EulerAnglesToQuaternion(new Degree(angleZ), new Degree(angleX), new Degree(angleY));

                writer.WriteAttributeString("qw", quat.w.ToString());
                writer.WriteAttributeString("qx", quat.x.ToString());
                writer.WriteAttributeString("qy", quat.y.ToString());
                writer.WriteAttributeString("qz", quat.z.ToString());
                writer.WriteEndElement();

                // SIZE
                reader.ReadToFollowing("SIZE");
                writer.WriteStartElement("scale");
                float subscale = float.Parse(reader["SCALE"], NumberFormatInfo) * (scale/10.0f);
                writer.WriteAttributeString("x", subscale.ToString());
                writer.WriteAttributeString("y", subscale.ToString());
                writer.WriteAttributeString("z", subscale.ToString());
                writer.WriteEndElement();
                
                // ENTITY
                writer.WriteStartElement("entity");
                writer.WriteAttributeString("meshfile", meshname);
                writer.WriteAttributeString("castshadows", chkTreeShadows.Checked.ToString());
                writer.WriteEndElement();
                
                writer.WriteEndElement();
                count++;
            }
            return count;
        }

        private void Log(string Text)
        {
            txtLog.AppendText(Text + Environment.NewLine);
        }
        
        /// <summary>
        /// Converts euler angles to quaternion
        /// </summary>
        /// <param name="YAngle">Yaw-Angle</param>
        /// <param name="XAngle">Pitch-Angle</param>
        /// <param name="ZAngle">Roll-Angle</param>
        /// <returns>orientation</returns>
        private Quaternion EulerAnglesToQuaternion(Degree YAngle, Degree XAngle, Degree ZAngle)
        {
            Radian radToYaw = new Radian(YAngle);
            Radian radToPitch = new Radian(XAngle);
            Radian radToRoll = new Radian(ZAngle);

            Quaternion orientation = Quaternion.IDENTITY;
            orientation *= new Quaternion(radToYaw, Vector3.UNIT_Y);
            orientation *= new Quaternion(radToPitch, Vector3.UNIT_X);
            orientation *= new Quaternion(radToRoll, Vector3.UNIT_Z);

            return orientation;
        }
        
    }
}
