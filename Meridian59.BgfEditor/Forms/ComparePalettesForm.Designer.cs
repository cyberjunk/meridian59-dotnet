namespace Meridian59.BgfEditor
{
    partial class ComparePalettesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPaletteLeft = new Meridian59.BgfEditor.Controls.ComboBoxPalette();
            this.picPaletteLeft = new Meridian59.BgfEditor.Controls.InterpolationModePictureBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPaletteRight = new Meridian59.BgfEditor.Controls.ComboBoxPalette();
            this.picPaletteRight = new Meridian59.BgfEditor.Controls.InterpolationModePictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteRight)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(684, 360);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.cbPaletteLeft);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.picPaletteLeft);
            this.splitContainer2.Size = new System.Drawing.Size(330, 360);
            this.splitContainer2.SplitterDistance = 60;
            this.splitContainer2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Palette";
            // 
            // cbPaletteLeft
            // 
            this.cbPaletteLeft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaletteLeft.FormattingEnabled = true;
            this.cbPaletteLeft.Items.AddRange(new object[] {
            "(000) - IDENTITY",
            "(001) - DBLUETOSKIN1",
            "(002) - DBLUETOSKIN2",
            "(003) - DBLUETOSKIN3",
            "(004) - DBLUETOSKIN4",
            "(005) - DBLUETOSICKGREEN",
            "(006) - DBLUETOSICKYELLOW",
            "(007) - DBLUETOGRAY",
            "(008) - DBLUETOLBLUE",
            "(009) - DBLUETOASHEN",
            "(010) - GRAYTOORANGE",
            "(011) - GRAYTODGREEN",
            "(012) - GRAYTOBGREEN",
            "(013) - GRAYTOSKY",
            "(014) - GRAYTODBLUE",
            "(015) - GRAYTOPURPLE",
            "(016) - GRAYTOGOLD",
            "(017) - GRAYTOBBLUE",
            "(018) - GRAYTORED",
            "(019) - GRAYTOLORANGE",
            "(020) - GRAYTOLGREEN",
            "(021) - GRAYTOLBGREEN",
            "(022) - GRAYTOLSKY",
            "(023) - GRAYTOLBLUE",
            "(024) - GRAYTOLPURPLE",
            "(025) - GRAYTOLGOLD",
            "(026) - GRAYTOSKIN1",
            "(027) - GRAYTOSKIN2",
            "(028) - GRAYTOSKIN3",
            "(029) - GRAYTOSKIN4",
            "(030) - GRAYTOSKIN5",
            "(031) - UNSET",
            "(032) - GRAYTOLBBLUE",
            "(033) - GRAYTOLRED",
            "(034) - GRAYTOKORANGE",
            "(035) - GRAYTOKGREEN",
            "(036) - GRAYTOKBGREEN",
            "(037) - GRAYTOKSKY",
            "(038) - GRAYTOKBLUE",
            "(039) - GRAYTOKPURPLE",
            "(040) - GRAYTOKGOLD",
            "(041) - GRAYTOKBBLUE",
            "(042) - GRAYTOKRED",
            "(043) - GRAYTOKGRAY",
            "(044) - GRAYTOBLACK",
            "(045) - GRAYTOOLDHAIR1",
            "(046) - GRAYTOOLDHAIR2",
            "(047) - GRAYTOOLDHAIR3",
            "(048) - GRAYTOPLATBLOND",
            "(049) - FILTERWHITE90",
            "(050) - FILTERWHITE80",
            "(051) - FILTERWHITE70",
            "(052) - UNSET",
            "(053) - UNSET",
            "(054) - FILTERBRIGHT1",
            "(055) - FILTERBRIGHT2",
            "(056) - FILTERBRIGHT3",
            "(057) - BLEND25YELLOW",
            "(058) - PURPLETOLBLUE",
            "(059) - PURPLETOBRED",
            "(060) - PURPLETOGREEN",
            "(061) - PURPLETOYELLOW",
            "(062) - UNSET",
            "(063) - UNSET",
            "(064) - UNSET",
            "(065) - BLEND10RED",
            "(066) - BLEND20RED",
            "(067) - BLEND30RED",
            "(068) - BLEND40RED",
            "(069) - BLEND50RED",
            "(070) - BLEND60RED",
            "(071) - BLEND70RED",
            "(072) - BLEND80RED",
            "(073) - BLEND90RED",
            "(074) - BLEND100RED",
            "(075) - UNSET",
            "(076) - UNSET",
            "(077) - FILTERRED",
            "(078) - FILTERBLUE",
            "(079) - FILTERGREEN",
            "(080) - UNSET",
            "(081) - BLEND25RED",
            "(082) - BLEND25BLUE",
            "(083) - BLEND25GREEN",
            "(084) - UNSET",
            "(085) - BLEND50BLUE",
            "(086) - BLEND50GREEN",
            "(087) - BLEND75RED",
            "(088) - BLEND75BLUE",
            "(089) - BLEND75GREEN",
            "(090) - REDTOBLACK",
            "(091) - BLUETOBLACK",
            "(092) - PURPLETOBLACK",
            "(093) - UNSET",
            "(094) - UNSET",
            "(095) - UNSET",
            "(096) - RAMPUP1",
            "(097) - RAMPUP2",
            "(098) - UNSET",
            "(099) - UNSET",
            "(100) - UNSET",
            "(101) - UNSET",
            "(102) - UNSET",
            "(103) - UNSET",
            "(104) - UNSET",
            "(105) - UNSET",
            "(106) - UNSET",
            "(107) - UNSET",
            "(108) - UNSET",
            "(109) - UNSET",
            "(110) - RAMPDOWN2",
            "(111) - RAMPDOWN1",
            "(112) - BLEND10WHITE",
            "(113) - BLEND20WHITE",
            "(114) - BLEND30WHITE",
            "(115) - BLEND40WHITE",
            "(116) - BLEND50WHITE",
            "(117) - BLEND60WHITE",
            "(118) - BLEND70WHITE",
            "(119) - BLEND80WHITE",
            "(120) - BLEND90WHITE",
            "(121) - BLEND100WHITE",
            "(122) - REDTODGREEN1",
            "(123) - REDTODGREEN2",
            "(124) - REDTODGREEN3",
            "(125) - REDTOBLACK1",
            "(126) - REDTOBLACK2",
            "(127) - REDTOBLACK3",
            "(128) - REDTODKBLACK1",
            "(129) - REDTODKBLACK2",
            "(130) - REDTODKBLACK3",
            "(131) - REDBLK_BLWHT",
            "(132) - BLBLK_REDWHT",
            "(133) - UNSET",
            "(134) - UNSET",
            "(135) - GUILDCOLOR0",
            "(136) - GUILDCOLOR1",
            "(137) - GUILDCOLOR2",
            "(138) - GUILDCOLOR3",
            "(139) - GUILDCOLOR4",
            "(140) - GUILDCOLOR5",
            "(141) - GUILDCOLOR6",
            "(142) - GUILDCOLOR7",
            "(143) - GUILDCOLOR8",
            "(144) - GUILDCOLOR9",
            "(145) - GUILDCOLOR10",
            "(146) - GUILDCOLOR11",
            "(147) - GUILDCOLOR12",
            "(148) - GUILDCOLOR13",
            "(149) - GUILDCOLOR14",
            "(150) - GUILDCOLOR15",
            "(151) - GUILDCOLOR16",
            "(152) - GUILDCOLOR17",
            "(153) - GUILDCOLOR18",
            "(154) - GUILDCOLOR19",
            "(155) - GUILDCOLOR20",
            "(156) - GUILDCOLOR21",
            "(157) - GUILDCOLOR22",
            "(158) - GUILDCOLOR23",
            "(159) - GUILDCOLOR24",
            "(160) - GUILDCOLOR25",
            "(161) - GUILDCOLOR26",
            "(162) - GUILDCOLOR27",
            "(163) - GUILDCOLOR28",
            "(164) - GUILDCOLOR29",
            "(165) - GUILDCOLOR30",
            "(166) - GUILDCOLOR31",
            "(167) - GUILDCOLOR32",
            "(168) - GUILDCOLOR33",
            "(169) - GUILDCOLOR34",
            "(170) - GUILDCOLOR35",
            "(171) - GUILDCOLOR36",
            "(172) - GUILDCOLOR37",
            "(173) - GUILDCOLOR38",
            "(174) - GUILDCOLOR39",
            "(175) - GUILDCOLOR40",
            "(176) - GUILDCOLOR41",
            "(177) - GUILDCOLOR42",
            "(178) - GUILDCOLOR43",
            "(179) - GUILDCOLOR44",
            "(180) - GUILDCOLOR45",
            "(181) - GUILDCOLOR46",
            "(182) - GUILDCOLOR47",
            "(183) - GUILDCOLOR48",
            "(184) - GUILDCOLOR49",
            "(185) - GUILDCOLOR50",
            "(186) - GUILDCOLOR51",
            "(187) - GUILDCOLOR52",
            "(188) - GUILDCOLOR53",
            "(189) - GUILDCOLOR54",
            "(190) - GUILDCOLOR55",
            "(191) - GUILDCOLOR56",
            "(192) - GUILDCOLOR57",
            "(193) - GUILDCOLOR58",
            "(194) - GUILDCOLOR59",
            "(195) - GUILDCOLOR60",
            "(196) - GUILDCOLOR61",
            "(197) - GUILDCOLOR62",
            "(198) - GUILDCOLOR63",
            "(199) - GUILDCOLOR64",
            "(200) - GUILDCOLOR65",
            "(201) - GUILDCOLOR66",
            "(202) - GUILDCOLOR67",
            "(203) - GUILDCOLOR68",
            "(204) - GUILDCOLOR69",
            "(205) - GUILDCOLOR70",
            "(206) - GUILDCOLOR71",
            "(207) - GUILDCOLOR72",
            "(208) - GUILDCOLOR73",
            "(209) - GUILDCOLOR74",
            "(210) - GUILDCOLOR75",
            "(211) - GUILDCOLOR76",
            "(212) - GUILDCOLOR77",
            "(213) - GUILDCOLOR78",
            "(214) - GUILDCOLOR79",
            "(215) - GUILDCOLOR80",
            "(216) - GUILDCOLOR81",
            "(217) - GUILDCOLOR82",
            "(218) - GUILDCOLOR83",
            "(219) - GUILDCOLOR84",
            "(220) - GUILDCOLOR85",
            "(221) - GUILDCOLOR86",
            "(222) - GUILDCOLOR87",
            "(223) - GUILDCOLOR88",
            "(224) - GUILDCOLOR89",
            "(225) - GUILDCOLOR90",
            "(226) - GUILDCOLOR91",
            "(227) - GUILDCOLOR92",
            "(228) - GUILDCOLOR93",
            "(229) - GUILDCOLOR94",
            "(230) - GUILDCOLOR95",
            "(231) - GUILDCOLOR96",
            "(232) - GUILDCOLOR97",
            "(233) - GUILDCOLOR98",
            "(234) - GUILDCOLOR99",
            "(235) - GUILDCOLOR100",
            "(236) - GUILDCOLOR101",
            "(237) - GUILDCOLOR102",
            "(238) - GUILDCOLOR103",
            "(239) - GUILDCOLOR104",
            "(240) - GUILDCOLOR105",
            "(241) - GUILDCOLOR106",
            "(242) - GUILDCOLOR107",
            "(243) - GUILDCOLOR108",
            "(244) - GUILDCOLOR109",
            "(245) - GUILDCOLOR110",
            "(246) - GUILDCOLOR111",
            "(247) - GUILDCOLOR112",
            "(248) - GUILDCOLOR113",
            "(249) - GUILDCOLOR114",
            "(250) - GUILDCOLOR115",
            "(251) - GUILDCOLOR116",
            "(252) - GUILDCOLOR117",
            "(253) - GUILDCOLOR118",
            "(254) - GUILDCOLOR119",
            "(255) - GUILDCOLOR120"});
            this.cbPaletteLeft.Location = new System.Drawing.Point(58, 24);
            this.cbPaletteLeft.Name = "cbPaletteLeft";
            this.cbPaletteLeft.Size = new System.Drawing.Size(250, 21);
            this.cbPaletteLeft.TabIndex = 0;
            this.cbPaletteLeft.SelectedIndexChanged += new System.EventHandler(this.cbPaletteLeft_SelectedIndexChanged);
            // 
            // picPaletteLeft
            // 
            this.picPaletteLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPaletteLeft.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.picPaletteLeft.Location = new System.Drawing.Point(0, 0);
            this.picPaletteLeft.Name = "picPaletteLeft";
            this.picPaletteLeft.Size = new System.Drawing.Size(330, 296);
            this.picPaletteLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPaletteLeft.TabIndex = 0;
            this.picPaletteLeft.TabStop = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.label2);
            this.splitContainer3.Panel1.Controls.Add(this.cbPaletteRight);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.picPaletteRight);
            this.splitContainer3.Size = new System.Drawing.Size(350, 360);
            this.splitContainer3.SplitterDistance = 60;
            this.splitContainer3.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Palette";
            // 
            // cbPaletteRight
            // 
            this.cbPaletteRight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaletteRight.FormattingEnabled = true;
            this.cbPaletteRight.Items.AddRange(new object[] {
            "(000) - IDENTITY",
            "(001) - DBLUETOSKIN1",
            "(002) - DBLUETOSKIN2",
            "(003) - DBLUETOSKIN3",
            "(004) - DBLUETOSKIN4",
            "(005) - DBLUETOSICKGREEN",
            "(006) - DBLUETOSICKYELLOW",
            "(007) - DBLUETOGRAY",
            "(008) - DBLUETOLBLUE",
            "(009) - DBLUETOASHEN",
            "(010) - GRAYTOORANGE",
            "(011) - GRAYTODGREEN",
            "(012) - GRAYTOBGREEN",
            "(013) - GRAYTOSKY",
            "(014) - GRAYTODBLUE",
            "(015) - GRAYTOPURPLE",
            "(016) - GRAYTOGOLD",
            "(017) - GRAYTOBBLUE",
            "(018) - GRAYTORED",
            "(019) - GRAYTOLORANGE",
            "(020) - GRAYTOLGREEN",
            "(021) - GRAYTOLBGREEN",
            "(022) - GRAYTOLSKY",
            "(023) - GRAYTOLBLUE",
            "(024) - GRAYTOLPURPLE",
            "(025) - GRAYTOLGOLD",
            "(026) - GRAYTOSKIN1",
            "(027) - GRAYTOSKIN2",
            "(028) - GRAYTOSKIN3",
            "(029) - GRAYTOSKIN4",
            "(030) - GRAYTOSKIN5",
            "(031) - UNSET",
            "(032) - GRAYTOLBBLUE",
            "(033) - GRAYTOLRED",
            "(034) - GRAYTOKORANGE",
            "(035) - GRAYTOKGREEN",
            "(036) - GRAYTOKBGREEN",
            "(037) - GRAYTOKSKY",
            "(038) - GRAYTOKBLUE",
            "(039) - GRAYTOKPURPLE",
            "(040) - GRAYTOKGOLD",
            "(041) - GRAYTOKBBLUE",
            "(042) - GRAYTOKRED",
            "(043) - GRAYTOKGRAY",
            "(044) - GRAYTOBLACK",
            "(045) - GRAYTOOLDHAIR1",
            "(046) - GRAYTOOLDHAIR2",
            "(047) - GRAYTOOLDHAIR3",
            "(048) - GRAYTOPLATBLOND",
            "(049) - FILTERWHITE90",
            "(050) - FILTERWHITE80",
            "(051) - FILTERWHITE70",
            "(052) - UNSET",
            "(053) - UNSET",
            "(054) - FILTERBRIGHT1",
            "(055) - FILTERBRIGHT2",
            "(056) - FILTERBRIGHT3",
            "(057) - BLEND25YELLOW",
            "(058) - PURPLETOLBLUE",
            "(059) - PURPLETOBRED",
            "(060) - PURPLETOGREEN",
            "(061) - PURPLETOYELLOW",
            "(062) - UNSET",
            "(063) - UNSET",
            "(064) - UNSET",
            "(065) - BLEND10RED",
            "(066) - BLEND20RED",
            "(067) - BLEND30RED",
            "(068) - BLEND40RED",
            "(069) - BLEND50RED",
            "(070) - BLEND60RED",
            "(071) - BLEND70RED",
            "(072) - BLEND80RED",
            "(073) - BLEND90RED",
            "(074) - BLEND100RED",
            "(075) - UNSET",
            "(076) - UNSET",
            "(077) - FILTERRED",
            "(078) - FILTERBLUE",
            "(079) - FILTERGREEN",
            "(080) - UNSET",
            "(081) - BLEND25RED",
            "(082) - BLEND25BLUE",
            "(083) - BLEND25GREEN",
            "(084) - UNSET",
            "(085) - BLEND50BLUE",
            "(086) - BLEND50GREEN",
            "(087) - BLEND75RED",
            "(088) - BLEND75BLUE",
            "(089) - BLEND75GREEN",
            "(090) - REDTOBLACK",
            "(091) - BLUETOBLACK",
            "(092) - PURPLETOBLACK",
            "(093) - UNSET",
            "(094) - UNSET",
            "(095) - UNSET",
            "(096) - RAMPUP1",
            "(097) - RAMPUP2",
            "(098) - UNSET",
            "(099) - UNSET",
            "(100) - UNSET",
            "(101) - UNSET",
            "(102) - UNSET",
            "(103) - UNSET",
            "(104) - UNSET",
            "(105) - UNSET",
            "(106) - UNSET",
            "(107) - UNSET",
            "(108) - UNSET",
            "(109) - UNSET",
            "(110) - RAMPDOWN2",
            "(111) - RAMPDOWN1",
            "(112) - BLEND10WHITE",
            "(113) - BLEND20WHITE",
            "(114) - BLEND30WHITE",
            "(115) - BLEND40WHITE",
            "(116) - BLEND50WHITE",
            "(117) - BLEND60WHITE",
            "(118) - BLEND70WHITE",
            "(119) - BLEND80WHITE",
            "(120) - BLEND90WHITE",
            "(121) - BLEND100WHITE",
            "(122) - REDTODGREEN1",
            "(123) - REDTODGREEN2",
            "(124) - REDTODGREEN3",
            "(125) - REDTOBLACK1",
            "(126) - REDTOBLACK2",
            "(127) - REDTOBLACK3",
            "(128) - REDTODKBLACK1",
            "(129) - REDTODKBLACK2",
            "(130) - REDTODKBLACK3",
            "(131) - REDBLK_BLWHT",
            "(132) - BLBLK_REDWHT",
            "(133) - UNSET",
            "(134) - UNSET",
            "(135) - GUILDCOLOR0",
            "(136) - GUILDCOLOR1",
            "(137) - GUILDCOLOR2",
            "(138) - GUILDCOLOR3",
            "(139) - GUILDCOLOR4",
            "(140) - GUILDCOLOR5",
            "(141) - GUILDCOLOR6",
            "(142) - GUILDCOLOR7",
            "(143) - GUILDCOLOR8",
            "(144) - GUILDCOLOR9",
            "(145) - GUILDCOLOR10",
            "(146) - GUILDCOLOR11",
            "(147) - GUILDCOLOR12",
            "(148) - GUILDCOLOR13",
            "(149) - GUILDCOLOR14",
            "(150) - GUILDCOLOR15",
            "(151) - GUILDCOLOR16",
            "(152) - GUILDCOLOR17",
            "(153) - GUILDCOLOR18",
            "(154) - GUILDCOLOR19",
            "(155) - GUILDCOLOR20",
            "(156) - GUILDCOLOR21",
            "(157) - GUILDCOLOR22",
            "(158) - GUILDCOLOR23",
            "(159) - GUILDCOLOR24",
            "(160) - GUILDCOLOR25",
            "(161) - GUILDCOLOR26",
            "(162) - GUILDCOLOR27",
            "(163) - GUILDCOLOR28",
            "(164) - GUILDCOLOR29",
            "(165) - GUILDCOLOR30",
            "(166) - GUILDCOLOR31",
            "(167) - GUILDCOLOR32",
            "(168) - GUILDCOLOR33",
            "(169) - GUILDCOLOR34",
            "(170) - GUILDCOLOR35",
            "(171) - GUILDCOLOR36",
            "(172) - GUILDCOLOR37",
            "(173) - GUILDCOLOR38",
            "(174) - GUILDCOLOR39",
            "(175) - GUILDCOLOR40",
            "(176) - GUILDCOLOR41",
            "(177) - GUILDCOLOR42",
            "(178) - GUILDCOLOR43",
            "(179) - GUILDCOLOR44",
            "(180) - GUILDCOLOR45",
            "(181) - GUILDCOLOR46",
            "(182) - GUILDCOLOR47",
            "(183) - GUILDCOLOR48",
            "(184) - GUILDCOLOR49",
            "(185) - GUILDCOLOR50",
            "(186) - GUILDCOLOR51",
            "(187) - GUILDCOLOR52",
            "(188) - GUILDCOLOR53",
            "(189) - GUILDCOLOR54",
            "(190) - GUILDCOLOR55",
            "(191) - GUILDCOLOR56",
            "(192) - GUILDCOLOR57",
            "(193) - GUILDCOLOR58",
            "(194) - GUILDCOLOR59",
            "(195) - GUILDCOLOR60",
            "(196) - GUILDCOLOR61",
            "(197) - GUILDCOLOR62",
            "(198) - GUILDCOLOR63",
            "(199) - GUILDCOLOR64",
            "(200) - GUILDCOLOR65",
            "(201) - GUILDCOLOR66",
            "(202) - GUILDCOLOR67",
            "(203) - GUILDCOLOR68",
            "(204) - GUILDCOLOR69",
            "(205) - GUILDCOLOR70",
            "(206) - GUILDCOLOR71",
            "(207) - GUILDCOLOR72",
            "(208) - GUILDCOLOR73",
            "(209) - GUILDCOLOR74",
            "(210) - GUILDCOLOR75",
            "(211) - GUILDCOLOR76",
            "(212) - GUILDCOLOR77",
            "(213) - GUILDCOLOR78",
            "(214) - GUILDCOLOR79",
            "(215) - GUILDCOLOR80",
            "(216) - GUILDCOLOR81",
            "(217) - GUILDCOLOR82",
            "(218) - GUILDCOLOR83",
            "(219) - GUILDCOLOR84",
            "(220) - GUILDCOLOR85",
            "(221) - GUILDCOLOR86",
            "(222) - GUILDCOLOR87",
            "(223) - GUILDCOLOR88",
            "(224) - GUILDCOLOR89",
            "(225) - GUILDCOLOR90",
            "(226) - GUILDCOLOR91",
            "(227) - GUILDCOLOR92",
            "(228) - GUILDCOLOR93",
            "(229) - GUILDCOLOR94",
            "(230) - GUILDCOLOR95",
            "(231) - GUILDCOLOR96",
            "(232) - GUILDCOLOR97",
            "(233) - GUILDCOLOR98",
            "(234) - GUILDCOLOR99",
            "(235) - GUILDCOLOR100",
            "(236) - GUILDCOLOR101",
            "(237) - GUILDCOLOR102",
            "(238) - GUILDCOLOR103",
            "(239) - GUILDCOLOR104",
            "(240) - GUILDCOLOR105",
            "(241) - GUILDCOLOR106",
            "(242) - GUILDCOLOR107",
            "(243) - GUILDCOLOR108",
            "(244) - GUILDCOLOR109",
            "(245) - GUILDCOLOR110",
            "(246) - GUILDCOLOR111",
            "(247) - GUILDCOLOR112",
            "(248) - GUILDCOLOR113",
            "(249) - GUILDCOLOR114",
            "(250) - GUILDCOLOR115",
            "(251) - GUILDCOLOR116",
            "(252) - GUILDCOLOR117",
            "(253) - GUILDCOLOR118",
            "(254) - GUILDCOLOR119",
            "(255) - GUILDCOLOR120"});
            this.cbPaletteRight.Location = new System.Drawing.Point(72, 24);
            this.cbPaletteRight.Name = "cbPaletteRight";
            this.cbPaletteRight.Size = new System.Drawing.Size(252, 21);
            this.cbPaletteRight.TabIndex = 2;
            this.cbPaletteRight.SelectedIndexChanged += new System.EventHandler(this.cbPaletteRight_SelectedIndexChanged);
            // 
            // picPaletteRight
            // 
            this.picPaletteRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPaletteRight.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.picPaletteRight.Location = new System.Drawing.Point(0, 0);
            this.picPaletteRight.Name = "picPaletteRight";
            this.picPaletteRight.Size = new System.Drawing.Size(350, 296);
            this.picPaletteRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPaletteRight.TabIndex = 0;
            this.picPaletteRight.TabStop = false;
            // 
            // ComparePalettesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 360);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ComparePalettesForm";
            this.Text = "Palettes";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteLeft)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPaletteRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Controls.ComboBoxPalette cbPaletteLeft;
        private Controls.InterpolationModePictureBox picPaletteLeft;
        private Controls.InterpolationModePictureBox picPaletteRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.ComboBoxPalette cbPaletteRight;
    }
}