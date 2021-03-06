// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/12/2009 9:49:40 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DetailedPointSymbolDialog
    /// </summary>
    public class DetailedPointSymbolControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        /// <summary>
        /// Occurs when the add to custom symbols button is pressed.
        /// </summary>
        public event EventHandler<PointSymbolizerEventArgs> AddToCustomSymbols;

        #endregion

        #region Private Variables

        private bool _disableUnitWarning;
        private bool _ignoreChanges;
        private IPointSymbolizer _original;
        private IPointSymbolizer _symbolizer;
        private AngleControl angleControl;
        private Button btnAddToCustom;
        private Button btnBrowseImage;
        private ColorButton cbColorCharacter;
        private ColorButton cbColorSimple;
        private ColorButton cbOutlineColor;
        private ColorButton cbOutlineColorPicture;
        private SymbolCollectionControl ccSymbols;
        private CharacterControl charCharacter;
        private CheckBox chkSmoothing;
        private CheckBox chkUseOutline;
        private CheckBox chkUseOutlinePicture;
        private FontFamilyControl cmbFontFamilly;
        private ComboBox cmbPointShape;
        private ComboBox cmbScaleMode;
        private ComboBox cmbSymbolType;
        private ComboBox cmbUnits;
        private DoubleBox dbxOffsetX;
        private DoubleBox dbxOffsetY;
        private DoubleBox dbxOutlineWidth;
        private DoubleBox dbxOutlineWidthPicture;
        private DoubleBox doubleBox10;
        private DoubleBox doubleBox11;
        private DoubleBox doubleBox7;
        private DoubleBox doubleBox8;
        private DoubleBox doubleBox9;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private GroupBox grpOffset;
        private GroupBox grpOutlinePicture;
        private GroupBox grpPlacement;
        private Label label10;
        private Label label2;
        private Label label3;
        private Label lblColorCharacter;
        private Label lblColorSimple;
        private Label lblFontFamily;
        private Label lblImage;
        private Label lblImageOpacity;
        private Label lblOutlineColor;
        private Label lblOutlineColorPicture;
        private Label lblPreview;
        private Label lblScaleMode;
        private Label lblSymbolType;
        private Label lblUnicode;
        private Label lblUnits;
        private SizeControl sizeControl;
        private RampSlider sldImageOpacity;
        private RampSlider sldOpacityCharacter;
        private RampSlider sldOpacitySimple;
        private RampSlider sldOutlineOpacity;
        private RampSlider sldOutlineOpacityPicture;
        private TabPage tabCharacter;
        private TabPage tabPicture;
        private TabPage tabSimple;
        private TabControl tabSymbolProperties;
        private TextBox txtImageFilename;
        private TextBox txtUnicode;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedPointSymbolControl));
            this.btnAddToCustom = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbUnits = new System.Windows.Forms.ComboBox();
            this.lblUnits = new System.Windows.Forms.Label();
            this.lblScaleMode = new System.Windows.Forms.Label();
            this.cmbScaleMode = new System.Windows.Forms.ComboBox();
            this.chkSmoothing = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.lblSymbolType = new System.Windows.Forms.Label();
            this.cmbSymbolType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.doubleBox7 = new DotSpatial.Symbology.Forms.DoubleBox();
            this.doubleBox8 = new DotSpatial.Symbology.Forms.DoubleBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.doubleBox10 = new DotSpatial.Symbology.Forms.DoubleBox();
            this.doubleBox11 = new DotSpatial.Symbology.Forms.DoubleBox();
            this.ccSymbols = new DotSpatial.Symbology.Forms.SymbolCollectionControl();
            this.doubleBox9 = new DotSpatial.Symbology.Forms.DoubleBox();
            this.tabPicture = new System.Windows.Forms.TabPage();
            this.lblImageOpacity = new System.Windows.Forms.Label();
            this.grpOutlinePicture = new System.Windows.Forms.GroupBox();
            this.cbOutlineColorPicture = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldOutlineOpacityPicture = new DotSpatial.Symbology.Forms.RampSlider();
            this.lblOutlineColorPicture = new System.Windows.Forms.Label();
            this.dbxOutlineWidthPicture = new DotSpatial.Symbology.Forms.DoubleBox();
            this.chkUseOutlinePicture = new System.Windows.Forms.CheckBox();
            this.btnBrowseImage = new System.Windows.Forms.Button();
            this.txtImageFilename = new System.Windows.Forms.TextBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.sldImageOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.tabCharacter = new System.Windows.Forms.TabPage();
            this.lblColorCharacter = new System.Windows.Forms.Label();
            this.txtUnicode = new System.Windows.Forms.TextBox();
            this.lblUnicode = new System.Windows.Forms.Label();
            this.lblFontFamily = new System.Windows.Forms.Label();
            this.cbColorCharacter = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldOpacityCharacter = new DotSpatial.Symbology.Forms.RampSlider();
            this.charCharacter = new DotSpatial.Symbology.Forms.CharacterControl();
            this.cmbFontFamilly = new DotSpatial.Symbology.Forms.FontFamilyControl();
            this.tabSimple = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbOutlineColor = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldOutlineOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.lblOutlineColor = new System.Windows.Forms.Label();
            this.dbxOutlineWidth = new DotSpatial.Symbology.Forms.DoubleBox();
            this.chkUseOutline = new System.Windows.Forms.CheckBox();
            this.cmbPointShape = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblColorSimple = new System.Windows.Forms.Label();
            this.cbColorSimple = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldOpacitySimple = new DotSpatial.Symbology.Forms.RampSlider();
            this.grpPlacement = new System.Windows.Forms.GroupBox();
            this.grpOffset = new System.Windows.Forms.GroupBox();
            this.dbxOffsetX = new DotSpatial.Symbology.Forms.DoubleBox();
            this.dbxOffsetY = new DotSpatial.Symbology.Forms.DoubleBox();
            this.angleControl = new DotSpatial.Symbology.Forms.AngleControl();
            this.sizeControl = new DotSpatial.Symbology.Forms.SizeControl();
            this.tabSymbolProperties = new System.Windows.Forms.TabControl();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPicture.SuspendLayout();
            this.grpOutlinePicture.SuspendLayout();
            this.tabCharacter.SuspendLayout();
            this.tabSimple.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpPlacement.SuspendLayout();
            this.grpOffset.SuspendLayout();
            this.tabSymbolProperties.SuspendLayout();
            this.SuspendLayout();
            //
            // btnAddToCustom
            //
            resources.ApplyResources(this.btnAddToCustom, "btnAddToCustom");
            this.btnAddToCustom.Name = "btnAddToCustom";
            this.btnAddToCustom.UseVisualStyleBackColor = true;
            this.btnAddToCustom.Click += new System.EventHandler(this.btnAddToCustom_Click);
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.cmbUnits);
            this.groupBox1.Controls.Add(this.lblUnits);
            this.groupBox1.Controls.Add(this.lblScaleMode);
            this.groupBox1.Controls.Add(this.cmbScaleMode);
            this.groupBox1.Controls.Add(this.chkSmoothing);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            //
            // cmbUnits
            //
            resources.ApplyResources(this.cmbUnits, "cmbUnits");
            this.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            this.cmbUnits.Items.AddRange(new object[] {
                                                          resources.GetString("cmbUnits.Items"),
                                                          resources.GetString("cmbUnits.Items1"),
                                                          resources.GetString("cmbUnits.Items2"),
                                                          resources.GetString("cmbUnits.Items3"),
                                                          resources.GetString("cmbUnits.Items4"),
                                                          resources.GetString("cmbUnits.Items5"),
                                                          resources.GetString("cmbUnits.Items6")});
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.SelectedIndexChanged += new System.EventHandler(this.cmbUnits_SelectedIndexChanged);
            //
            // lblUnits
            //
            resources.ApplyResources(this.lblUnits, "lblUnits");
            this.lblUnits.Name = "lblUnits";
            //
            // lblScaleMode
            //
            resources.ApplyResources(this.lblScaleMode, "lblScaleMode");
            this.lblScaleMode.Name = "lblScaleMode";
            //
            // cmbScaleMode
            //
            resources.ApplyResources(this.cmbScaleMode, "cmbScaleMode");
            this.cmbScaleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScaleMode.FormattingEnabled = true;
            this.cmbScaleMode.Items.AddRange(new object[] {
                                                              resources.GetString("cmbScaleMode.Items"),
                                                              resources.GetString("cmbScaleMode.Items1"),
                                                              resources.GetString("cmbScaleMode.Items2")});
            this.cmbScaleMode.Name = "cmbScaleMode";
            this.cmbScaleMode.SelectedIndexChanged += new System.EventHandler(this.cmbScaleMode_SelectedIndexChanged);
            //
            // chkSmoothing
            //
            resources.ApplyResources(this.chkSmoothing, "chkSmoothing");
            this.chkSmoothing.Name = "chkSmoothing";
            this.chkSmoothing.UseVisualStyleBackColor = true;
            this.chkSmoothing.CheckedChanged += new System.EventHandler(this.chkSmoothing_CheckedChanged);
            //
            // label3
            //
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            //
            // lblPreview
            //
            this.lblPreview.BackColor = System.Drawing.Color.White;
            this.lblPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.Name = "lblPreview";
            //
            // lblSymbolType
            //
            resources.ApplyResources(this.lblSymbolType, "lblSymbolType");
            this.lblSymbolType.Name = "lblSymbolType";
            //
            // cmbSymbolType
            //
            this.cmbSymbolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSymbolType.FormattingEnabled = true;
            this.cmbSymbolType.Items.AddRange(new object[] {
                                                               resources.GetString("cmbSymbolType.Items"),
                                                               resources.GetString("cmbSymbolType.Items1"),
                                                               resources.GetString("cmbSymbolType.Items2")});
            resources.ApplyResources(this.cmbSymbolType, "cmbSymbolType");
            this.cmbSymbolType.Name = "cmbSymbolType";
            this.cmbSymbolType.SelectedIndexChanged += new System.EventHandler(this.cmbSymbolType_SelectedIndexChanged);
            //
            // label10
            //
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            //
            // groupBox6
            //
            this.groupBox6.Controls.Add(this.doubleBox7);
            this.groupBox6.Controls.Add(this.doubleBox8);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            //
            // doubleBox7
            //
            this.doubleBox7.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox7.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.doubleBox7, "doubleBox7");
            this.doubleBox7.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                          "ating point value.";
            this.doubleBox7.IsValid = true;
            this.doubleBox7.Name = "doubleBox7";
            this.doubleBox7.NumberFormat = null;
            this.doubleBox7.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox7.Value = 0D;
            //
            // doubleBox8
            //
            this.doubleBox8.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox8.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.doubleBox8, "doubleBox8");
            this.doubleBox8.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                          "ating point value.";
            this.doubleBox8.IsValid = true;
            this.doubleBox8.Name = "doubleBox8";
            this.doubleBox8.NumberFormat = null;
            this.doubleBox8.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox8.Value = 0D;
            //
            // groupBox7
            //
            this.groupBox7.Controls.Add(this.doubleBox10);
            this.groupBox7.Controls.Add(this.doubleBox11);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            //
            // doubleBox10
            //
            this.doubleBox10.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox10.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.doubleBox10, "doubleBox10");
            this.doubleBox10.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                           "ating point value.";
            this.doubleBox10.IsValid = true;
            this.doubleBox10.Name = "doubleBox10";
            this.doubleBox10.NumberFormat = null;
            this.doubleBox10.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox10.Value = 0D;
            //
            // doubleBox11
            //
            this.doubleBox11.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox11.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.doubleBox11, "doubleBox11");
            this.doubleBox11.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                           "ating point value.";
            this.doubleBox11.IsValid = true;
            this.doubleBox11.Name = "doubleBox11";
            this.doubleBox11.NumberFormat = null;
            this.doubleBox11.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox11.Value = 0D;
            //
            // ccSymbols
            //
            resources.ApplyResources(this.ccSymbols, "ccSymbols");
            this.ccSymbols.ItemHeight = 40;
            this.ccSymbols.Name = "ccSymbols";
            this.ccSymbols.ScaleMode = DotSpatial.Symbology.ScaleMode.Simple;
            //
            // doubleBox9
            //
            this.doubleBox9.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox9.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.doubleBox9, "doubleBox9");
            this.doubleBox9.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                          "ating point value.";
            this.doubleBox9.IsValid = true;
            this.doubleBox9.Name = "doubleBox9";
            this.doubleBox9.NumberFormat = null;
            this.doubleBox9.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox9.Value = 0D;
            //
            // tabPicture
            //
            this.tabPicture.Controls.Add(this.lblImageOpacity);
            this.tabPicture.Controls.Add(this.grpOutlinePicture);
            this.tabPicture.Controls.Add(this.btnBrowseImage);
            this.tabPicture.Controls.Add(this.txtImageFilename);
            this.tabPicture.Controls.Add(this.lblImage);
            this.tabPicture.Controls.Add(this.sldImageOpacity);
            resources.ApplyResources(this.tabPicture, "tabPicture");
            this.tabPicture.Name = "tabPicture";
            this.tabPicture.UseVisualStyleBackColor = true;
            //
            // lblImageOpacity
            //
            resources.ApplyResources(this.lblImageOpacity, "lblImageOpacity");
            this.lblImageOpacity.Name = "lblImageOpacity";
            //
            // grpOutlinePicture
            //
            this.grpOutlinePicture.Controls.Add(this.cbOutlineColorPicture);
            this.grpOutlinePicture.Controls.Add(this.sldOutlineOpacityPicture);
            this.grpOutlinePicture.Controls.Add(this.lblOutlineColorPicture);
            this.grpOutlinePicture.Controls.Add(this.dbxOutlineWidthPicture);
            this.grpOutlinePicture.Controls.Add(this.chkUseOutlinePicture);
            resources.ApplyResources(this.grpOutlinePicture, "grpOutlinePicture");
            this.grpOutlinePicture.Name = "grpOutlinePicture";
            this.grpOutlinePicture.TabStop = false;
            //
            // cbOutlineColorPicture
            //
            this.cbOutlineColorPicture.BevelRadius = 4;
            this.cbOutlineColorPicture.Color = System.Drawing.Color.Blue;
            this.cbOutlineColorPicture.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbOutlineColorPicture, "cbOutlineColorPicture");
            this.cbOutlineColorPicture.Name = "cbOutlineColorPicture";
            this.cbOutlineColorPicture.RoundingRadius = 10;
            this.cbOutlineColorPicture.ColorChanged += new System.EventHandler(this.cbOutlineColorPicture_ColorChanged);
            //
            // sldOutlineOpacityPicture
            //
            this.sldOutlineOpacityPicture.ColorButton = null;
            this.sldOutlineOpacityPicture.FlipRamp = false;
            this.sldOutlineOpacityPicture.FlipText = false;
            this.sldOutlineOpacityPicture.InvertRamp = false;
            resources.ApplyResources(this.sldOutlineOpacityPicture, "sldOutlineOpacityPicture");
            this.sldOutlineOpacityPicture.Maximum = 1D;
            this.sldOutlineOpacityPicture.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sldOutlineOpacityPicture.Minimum = 0D;
            this.sldOutlineOpacityPicture.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldOutlineOpacityPicture.Name = "sldOutlineOpacityPicture";
            this.sldOutlineOpacityPicture.NumberFormat = null;
            this.sldOutlineOpacityPicture.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldOutlineOpacityPicture.RampRadius = 8F;
            this.sldOutlineOpacityPicture.RampText = "Opacity";
            this.sldOutlineOpacityPicture.RampTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.sldOutlineOpacityPicture.RampTextBehindRamp = true;
            this.sldOutlineOpacityPicture.RampTextColor = System.Drawing.Color.Black;
            this.sldOutlineOpacityPicture.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldOutlineOpacityPicture.ShowMaximum = true;
            this.sldOutlineOpacityPicture.ShowMinimum = true;
            this.sldOutlineOpacityPicture.ShowTicks = true;
            this.sldOutlineOpacityPicture.ShowValue = false;
            this.sldOutlineOpacityPicture.SliderColor = System.Drawing.Color.Blue;
            this.sldOutlineOpacityPicture.SliderRadius = 4F;
            this.sldOutlineOpacityPicture.TickColor = System.Drawing.Color.DarkGray;
            this.sldOutlineOpacityPicture.TickSpacing = 5F;
            this.sldOutlineOpacityPicture.Value = 0D;
            this.sldOutlineOpacityPicture.ValueChanged += new System.EventHandler(this.sldOutlineOpacityPicture_ValueChanged);
            //
            // lblOutlineColorPicture
            //
            resources.ApplyResources(this.lblOutlineColorPicture, "lblOutlineColorPicture");
            this.lblOutlineColorPicture.Name = "lblOutlineColorPicture";
            //
            // dbxOutlineWidthPicture
            //
            this.dbxOutlineWidthPicture.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxOutlineWidthPicture.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxOutlineWidthPicture, "dbxOutlineWidthPicture");
            this.dbxOutlineWidthPicture.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                                      "ating point value.";
            this.dbxOutlineWidthPicture.IsValid = true;
            this.dbxOutlineWidthPicture.Name = "dbxOutlineWidthPicture";
            this.dbxOutlineWidthPicture.NumberFormat = null;
            this.dbxOutlineWidthPicture.RegularHelp = "Enter a double precision floating point value.";
            this.dbxOutlineWidthPicture.Value = 0D;
            this.dbxOutlineWidthPicture.TextChanged += new System.EventHandler(this.dbxOutlineWidthPicture_TextChanged);
            //
            // chkUseOutlinePicture
            //
            resources.ApplyResources(this.chkUseOutlinePicture, "chkUseOutlinePicture");
            this.chkUseOutlinePicture.Name = "chkUseOutlinePicture";
            this.chkUseOutlinePicture.UseVisualStyleBackColor = true;
            this.chkUseOutlinePicture.CheckedChanged += new System.EventHandler(this.chkUseOutlinePicture_CheckedChanged);
            //
            // btnBrowseImage
            //
            resources.ApplyResources(this.btnBrowseImage, "btnBrowseImage");
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.UseVisualStyleBackColor = true;
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            //
            // txtImageFilename
            //
            resources.ApplyResources(this.txtImageFilename, "txtImageFilename");
            this.txtImageFilename.Name = "txtImageFilename";
            //
            // lblImage
            //
            resources.ApplyResources(this.lblImage, "lblImage");
            this.lblImage.Name = "lblImage";
            //
            // sldImageOpacity
            //
            this.sldImageOpacity.ColorButton = null;
            this.sldImageOpacity.FlipRamp = false;
            this.sldImageOpacity.FlipText = false;
            this.sldImageOpacity.InvertRamp = false;
            resources.ApplyResources(this.sldImageOpacity, "sldImageOpacity");
            this.sldImageOpacity.Maximum = 1D;
            this.sldImageOpacity.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sldImageOpacity.Minimum = 0D;
            this.sldImageOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldImageOpacity.Name = "sldImageOpacity";
            this.sldImageOpacity.NumberFormat = null;
            this.sldImageOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldImageOpacity.RampRadius = 8F;
            this.sldImageOpacity.RampText = "Opacity";
            this.sldImageOpacity.RampTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.sldImageOpacity.RampTextBehindRamp = true;
            this.sldImageOpacity.RampTextColor = System.Drawing.Color.Black;
            this.sldImageOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldImageOpacity.ShowMaximum = true;
            this.sldImageOpacity.ShowMinimum = true;
            this.sldImageOpacity.ShowTicks = true;
            this.sldImageOpacity.ShowValue = false;
            this.sldImageOpacity.SliderColor = System.Drawing.Color.Blue;
            this.sldImageOpacity.SliderRadius = 4F;
            this.sldImageOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.sldImageOpacity.TickSpacing = 5F;
            this.sldImageOpacity.Value = 0D;
            this.sldImageOpacity.ValueChanged += new System.EventHandler(this.sldImageOpacity_ValueChanged);
            //
            // tabCharacter
            //
            this.tabCharacter.Controls.Add(this.lblColorCharacter);
            this.tabCharacter.Controls.Add(this.txtUnicode);
            this.tabCharacter.Controls.Add(this.lblUnicode);
            this.tabCharacter.Controls.Add(this.lblFontFamily);
            this.tabCharacter.Controls.Add(this.cbColorCharacter);
            this.tabCharacter.Controls.Add(this.sldOpacityCharacter);
            this.tabCharacter.Controls.Add(this.charCharacter);
            this.tabCharacter.Controls.Add(this.cmbFontFamilly);
            resources.ApplyResources(this.tabCharacter, "tabCharacter");
            this.tabCharacter.Name = "tabCharacter";
            this.tabCharacter.UseVisualStyleBackColor = true;
            //
            // lblColorCharacter
            //
            resources.ApplyResources(this.lblColorCharacter, "lblColorCharacter");
            this.lblColorCharacter.Name = "lblColorCharacter";
            //
            // txtUnicode
            //
            resources.ApplyResources(this.txtUnicode, "txtUnicode");
            this.txtUnicode.Name = "txtUnicode";
            //
            // lblUnicode
            //
            resources.ApplyResources(this.lblUnicode, "lblUnicode");
            this.lblUnicode.Name = "lblUnicode";
            //
            // lblFontFamily
            //
            resources.ApplyResources(this.lblFontFamily, "lblFontFamily");
            this.lblFontFamily.Name = "lblFontFamily";
            //
            // cbColorCharacter
            //
            this.cbColorCharacter.BevelRadius = 4;
            this.cbColorCharacter.Color = System.Drawing.Color.Blue;
            this.cbColorCharacter.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbColorCharacter, "cbColorCharacter");
            this.cbColorCharacter.Name = "cbColorCharacter";
            this.cbColorCharacter.RoundingRadius = 10;
            this.cbColorCharacter.ColorChanged += new System.EventHandler(this.cbColorCharacter_ColorChanged);
            //
            // sldOpacityCharacter
            //
            this.sldOpacityCharacter.ColorButton = null;
            this.sldOpacityCharacter.FlipRamp = false;
            this.sldOpacityCharacter.FlipText = false;
            this.sldOpacityCharacter.InvertRamp = false;
            resources.ApplyResources(this.sldOpacityCharacter, "sldOpacityCharacter");
            this.sldOpacityCharacter.Maximum = 1D;
            this.sldOpacityCharacter.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sldOpacityCharacter.Minimum = 0D;
            this.sldOpacityCharacter.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldOpacityCharacter.Name = "sldOpacityCharacter";
            this.sldOpacityCharacter.NumberFormat = null;
            this.sldOpacityCharacter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldOpacityCharacter.RampRadius = 12F;
            this.sldOpacityCharacter.RampText = "Opacity";
            this.sldOpacityCharacter.RampTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.sldOpacityCharacter.RampTextBehindRamp = true;
            this.sldOpacityCharacter.RampTextColor = System.Drawing.Color.Black;
            this.sldOpacityCharacter.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldOpacityCharacter.ShowMaximum = false;
            this.sldOpacityCharacter.ShowMinimum = false;
            this.sldOpacityCharacter.ShowTicks = false;
            this.sldOpacityCharacter.ShowValue = false;
            this.sldOpacityCharacter.SliderColor = System.Drawing.Color.Blue;
            this.sldOpacityCharacter.SliderRadius = 4F;
            this.sldOpacityCharacter.TickColor = System.Drawing.Color.DarkGray;
            this.sldOpacityCharacter.TickSpacing = 5F;
            this.sldOpacityCharacter.Value = 0D;
            this.sldOpacityCharacter.ValueChanged += new System.EventHandler(this.sldOpacityCharacter_ValueChanged);
            //
            // charCharacter
            //
            this.charCharacter.CellSize = new System.Drawing.Size(22, 22);
            this.charCharacter.ControlRectangle = new System.Drawing.Rectangle(0, 0, 235, 149);
            this.charCharacter.DynamicColumns = true;
            resources.ApplyResources(this.charCharacter, "charCharacter");
            this.charCharacter.IsInitialized = false;
            this.charCharacter.IsSelected = false;
            this.charCharacter.Name = "charCharacter";
            this.charCharacter.NumColumns = 9;
            this.charCharacter.SelectedChar = ((byte)(0));
            this.charCharacter.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            this.charCharacter.SelectionForeColor = System.Drawing.Color.White;
            this.charCharacter.TypeSet = ((byte)(0));
            this.charCharacter.VerticalScrollEnabled = true;
            this.charCharacter.PopupClicked += new System.EventHandler(this.charCharacter_PopupClicked);
            //
            // cmbFontFamilly
            //
            resources.ApplyResources(this.cmbFontFamilly, "cmbFontFamilly");
            this.cmbFontFamilly.Name = "cmbFontFamilly";
            this.cmbFontFamilly.SelectedItemChanged += new System.EventHandler(this.cmbFontFamilly_SelectedItemChanged);
            //
            // tabSimple
            //
            this.tabSimple.Controls.Add(this.groupBox2);
            this.tabSimple.Controls.Add(this.cmbPointShape);
            this.tabSimple.Controls.Add(this.label2);
            this.tabSimple.Controls.Add(this.lblColorSimple);
            this.tabSimple.Controls.Add(this.cbColorSimple);
            this.tabSimple.Controls.Add(this.sldOpacitySimple);
            resources.ApplyResources(this.tabSimple, "tabSimple");
            this.tabSimple.Name = "tabSimple";
            this.tabSimple.UseVisualStyleBackColor = true;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.cbOutlineColor);
            this.groupBox2.Controls.Add(this.sldOutlineOpacity);
            this.groupBox2.Controls.Add(this.lblOutlineColor);
            this.groupBox2.Controls.Add(this.dbxOutlineWidth);
            this.groupBox2.Controls.Add(this.chkUseOutline);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            //
            // cbOutlineColor
            //
            this.cbOutlineColor.BevelRadius = 4;
            this.cbOutlineColor.Color = System.Drawing.Color.Blue;
            this.cbOutlineColor.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbOutlineColor, "cbOutlineColor");
            this.cbOutlineColor.Name = "cbOutlineColor";
            this.cbOutlineColor.RoundingRadius = 10;
            this.cbOutlineColor.ColorChanged += new System.EventHandler(this.cbOutlineColor_ColorChanged);
            //
            // sldOutlineOpacity
            //
            this.sldOutlineOpacity.ColorButton = null;
            this.sldOutlineOpacity.FlipRamp = false;
            this.sldOutlineOpacity.FlipText = false;
            this.sldOutlineOpacity.InvertRamp = false;
            resources.ApplyResources(this.sldOutlineOpacity, "sldOutlineOpacity");
            this.sldOutlineOpacity.Maximum = 1D;
            this.sldOutlineOpacity.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sldOutlineOpacity.Minimum = 0D;
            this.sldOutlineOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldOutlineOpacity.Name = "sldOutlineOpacity";
            this.sldOutlineOpacity.NumberFormat = null;
            this.sldOutlineOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldOutlineOpacity.RampRadius = 8F;
            this.sldOutlineOpacity.RampText = "Opacity";
            this.sldOutlineOpacity.RampTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.sldOutlineOpacity.RampTextBehindRamp = true;
            this.sldOutlineOpacity.RampTextColor = System.Drawing.Color.Black;
            this.sldOutlineOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldOutlineOpacity.ShowMaximum = true;
            this.sldOutlineOpacity.ShowMinimum = true;
            this.sldOutlineOpacity.ShowTicks = true;
            this.sldOutlineOpacity.ShowValue = false;
            this.sldOutlineOpacity.SliderColor = System.Drawing.Color.SteelBlue;
            this.sldOutlineOpacity.SliderRadius = 4F;
            this.sldOutlineOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.sldOutlineOpacity.TickSpacing = 5F;
            this.sldOutlineOpacity.Value = 0D;
            this.sldOutlineOpacity.ValueChanged += new System.EventHandler(this.sldOutlineOpacity_ValueChanged);
            //
            // lblOutlineColor
            //
            resources.ApplyResources(this.lblOutlineColor, "lblOutlineColor");
            this.lblOutlineColor.Name = "lblOutlineColor";
            //
            // dbxOutlineWidth
            //
            this.dbxOutlineWidth.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxOutlineWidth.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxOutlineWidth, "dbxOutlineWidth");
            this.dbxOutlineWidth.CausesValidation = false;
            this.dbxOutlineWidth.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                               "ating point value.";
            this.dbxOutlineWidth.IsValid = true;
            this.dbxOutlineWidth.Name = "dbxOutlineWidth";
            this.dbxOutlineWidth.NumberFormat = null;
            this.dbxOutlineWidth.RegularHelp = "Enter a double precision floating point value.";
            this.dbxOutlineWidth.Value = 0D;
            this.dbxOutlineWidth.TextChanged += new System.EventHandler(this.dbxOutlineWidth_TextChanged);
            //
            // chkUseOutline
            //
            resources.ApplyResources(this.chkUseOutline, "chkUseOutline");
            this.chkUseOutline.Name = "chkUseOutline";
            this.chkUseOutline.UseVisualStyleBackColor = true;
            this.chkUseOutline.CheckedChanged += new System.EventHandler(this.chkUseOutline_CheckedChanged);
            //
            // cmbPointShape
            //
            this.cmbPointShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPointShape.FormattingEnabled = true;
            this.cmbPointShape.Items.AddRange(new object[] {
                                                               resources.GetString("cmbPointShape.Items"),
                                                               resources.GetString("cmbPointShape.Items1"),
                                                               resources.GetString("cmbPointShape.Items2"),
                                                               resources.GetString("cmbPointShape.Items3"),
                                                               resources.GetString("cmbPointShape.Items4"),
                                                               resources.GetString("cmbPointShape.Items5"),
                                                               resources.GetString("cmbPointShape.Items6")});
            resources.ApplyResources(this.cmbPointShape, "cmbPointShape");
            this.cmbPointShape.Name = "cmbPointShape";
            this.cmbPointShape.SelectedIndexChanged += new System.EventHandler(this.cmbPointShape_SelectedIndexChanged);
            //
            // label2
            //
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            //
            // lblColorSimple
            //
            resources.ApplyResources(this.lblColorSimple, "lblColorSimple");
            this.lblColorSimple.Name = "lblColorSimple";
            //
            // cbColorSimple
            //
            this.cbColorSimple.BevelRadius = 4;
            this.cbColorSimple.Color = System.Drawing.Color.Blue;
            this.cbColorSimple.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbColorSimple, "cbColorSimple");
            this.cbColorSimple.Name = "cbColorSimple";
            this.cbColorSimple.RoundingRadius = 10;
            this.cbColorSimple.ColorChanged += new System.EventHandler(this.cbColorSimple_ColorChanged);
            //
            // sldOpacitySimple
            //
            this.sldOpacitySimple.ColorButton = null;
            this.sldOpacitySimple.FlipRamp = false;
            this.sldOpacitySimple.FlipText = false;
            this.sldOpacitySimple.InvertRamp = false;
            resources.ApplyResources(this.sldOpacitySimple, "sldOpacitySimple");
            this.sldOpacitySimple.Maximum = 1D;
            this.sldOpacitySimple.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sldOpacitySimple.Minimum = 0D;
            this.sldOpacitySimple.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldOpacitySimple.Name = "sldOpacitySimple";
            this.sldOpacitySimple.NumberFormat = null;
            this.sldOpacitySimple.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldOpacitySimple.RampRadius = 8F;
            this.sldOpacitySimple.RampText = "Opacity";
            this.sldOpacitySimple.RampTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.sldOpacitySimple.RampTextBehindRamp = true;
            this.sldOpacitySimple.RampTextColor = System.Drawing.Color.Black;
            this.sldOpacitySimple.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldOpacitySimple.ShowMaximum = true;
            this.sldOpacitySimple.ShowMinimum = true;
            this.sldOpacitySimple.ShowTicks = true;
            this.sldOpacitySimple.ShowValue = false;
            this.sldOpacitySimple.SliderColor = System.Drawing.Color.SteelBlue;
            this.sldOpacitySimple.SliderRadius = 4F;
            this.sldOpacitySimple.TickColor = System.Drawing.Color.DarkGray;
            this.sldOpacitySimple.TickSpacing = 5F;
            this.sldOpacitySimple.Value = 0D;
            this.sldOpacitySimple.ValueChanged += new System.EventHandler(this.sldOpacitySimple_ValueChanged);
            //
            // grpPlacement
            //
            resources.ApplyResources(this.grpPlacement, "grpPlacement");
            this.grpPlacement.Controls.Add(this.grpOffset);
            this.grpPlacement.Controls.Add(this.angleControl);
            this.grpPlacement.Controls.Add(this.sizeControl);
            this.grpPlacement.Name = "grpPlacement";
            this.grpPlacement.TabStop = false;
            //
            // grpOffset
            //
            resources.ApplyResources(this.grpOffset, "grpOffset");
            this.grpOffset.Controls.Add(this.dbxOffsetX);
            this.grpOffset.Controls.Add(this.dbxOffsetY);
            this.grpOffset.Name = "grpOffset";
            this.grpOffset.TabStop = false;
            //
            // dbxOffsetX
            //
            resources.ApplyResources(this.dbxOffsetX, "dbxOffsetX");
            this.dbxOffsetX.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxOffsetX.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxOffsetX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                          "ating point value.";
            this.dbxOffsetX.IsValid = true;
            this.dbxOffsetX.Name = "dbxOffsetX";
            this.dbxOffsetX.NumberFormat = null;
            this.dbxOffsetX.RegularHelp = "Enter a double precision floating point value.";
            this.dbxOffsetX.Value = 0D;
            this.dbxOffsetX.TextChanged += new System.EventHandler(this.dbxOffsetXSimple_TextChanged);
            //
            // dbxOffsetY
            //
            resources.ApplyResources(this.dbxOffsetY, "dbxOffsetY");
            this.dbxOffsetY.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxOffsetY.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxOffsetY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                          "ating point value.";
            this.dbxOffsetY.IsValid = true;
            this.dbxOffsetY.Name = "dbxOffsetY";
            this.dbxOffsetY.NumberFormat = null;
            this.dbxOffsetY.RegularHelp = "Enter a double precision floating point value.";
            this.dbxOffsetY.Value = 0D;
            this.dbxOffsetY.TextChanged += new System.EventHandler(this.dbxOffsetYSimple_TextChanged);
            //
            // angleControl
            //
            this.angleControl.Angle = 0;
            this.angleControl.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.angleControl, "angleControl");
            this.angleControl.Clockwise = false;
            this.angleControl.KnobColor = System.Drawing.Color.SteelBlue;
            this.angleControl.Name = "angleControl";
            this.angleControl.StartAngle = 0;
            this.angleControl.AngleChanged += new System.EventHandler(this.angleControlSimple_AngleChanged);
            //
            // sizeControl
            //
            resources.ApplyResources(this.sizeControl, "sizeControl");
            this.sizeControl.Name = "sizeControl";
            this.sizeControl.SelectedSizeChanged += new System.EventHandler(this.sizeControlSimple_SelectedSizeChanged);
            //
            // tabSymbolProperties
            //
            resources.ApplyResources(this.tabSymbolProperties, "tabSymbolProperties");
            this.tabSymbolProperties.Controls.Add(this.tabSimple);
            this.tabSymbolProperties.Controls.Add(this.tabCharacter);
            this.tabSymbolProperties.Controls.Add(this.tabPicture);
            this.tabSymbolProperties.Name = "tabSymbolProperties";
            this.tabSymbolProperties.SelectedIndex = 0;
            //
            // DetailedPointSymbolControl
            //
            this.Controls.Add(this.grpPlacement);
            this.Controls.Add(this.btnAddToCustom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabSymbolProperties);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.lblSymbolType);
            this.Controls.Add(this.cmbSymbolType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ccSymbols);
            this.Name = "DetailedPointSymbolControl";
            resources.ApplyResources(this, "$this");
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabPicture.ResumeLayout(false);
            this.tabPicture.PerformLayout();
            this.grpOutlinePicture.ResumeLayout(false);
            this.grpOutlinePicture.PerformLayout();
            this.tabCharacter.ResumeLayout(false);
            this.tabCharacter.PerformLayout();
            this.tabSimple.ResumeLayout(false);
            this.tabSimple.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpPlacement.ResumeLayout(false);
            this.grpOffset.ResumeLayout(false);
            this.grpOffset.PerformLayout();
            this.tabSymbolProperties.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        public DetailedPointSymbolControl()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new DetailedPointSymbolDialog that will update the specified original symbol
        /// by copying the new aspects to it only if the Apply Changes, or Ok buttons are used.
        /// </summary>
        /// <param name="original">The original IPointSymbolizer to modify.</param>
        public DetailedPointSymbolControl(IPointSymbolizer original)
        {
            Configure();
            Initialize(original);
        }

        private void Configure()
        {
            InitializeComponent();
            ccSymbols.AddClicked += ccSymbols_AddClicked;
            ccSymbols.SelectedItemChanged += ccSymbols_SelectedItemChanged;
            ccSymbols.ListChanged += ccSymbols_ListChanged;
            lblPreview.Paint += lblPreview_Paint;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the existing control with the new symbolizer
        /// </summary>
        /// <param name="original"></param>
        public void Initialize(IPointSymbolizer original)
        {
            _original = original;
            _symbolizer = original.Copy();
            ccSymbols.Symbols = _symbolizer.Symbols;
            ccSymbols.RefreshList();
            if (_symbolizer.Symbols.Count > 0)
            {
                ccSymbols.SelectedSymbol = _symbolizer.Symbols[0];
            }
            UpdatePreview();
            UpdateSymbolControls();
        }

        /// <summary>
        /// Applies the specified changes
        /// </summary>
        public void ApplyChanges()
        {
            OnApplyChanges();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current (copied) symbolizer or initializes this control to work with the
        /// specified symbolizer as the original.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IPointSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set
            {
                if (value != null) Initialize(value);
            }
        }

        #endregion

        #region Events

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            // This duplicates the content from the edit copy, but leaves the original object reference intact so that the map updates.
            _original.CopyProperties(_symbolizer);
            if (ChangesApplied != null) ChangesApplied(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the AddtoCustomSymbols event
        /// </summary>
        protected virtual void OnAddToCustomSymbols()
        {
            if (AddToCustomSymbols != null) AddToCustomSymbols(this, new PointSymbolizerEventArgs(_symbolizer));
        }

        #endregion

        #region Event Handlers

        #region Dialog Buttons

        #endregion

        #region General

        private void ccSymbols_AddClicked(object sender, EventArgs e)
        {
            ISymbol s = null;
            string type = cmbSymbolType.SelectedItem.ToString();
            switch (type)
            {
                case "Simple": s = new SimpleSymbol(); break;
                case "Character": s = new CharacterSymbol(); break;
                case "Picture": s = new PictureSymbol(); break;
            }
            if (s != null) ccSymbols.Symbols.Add(s);
            ccSymbols.RefreshList();
            UpdatePreview();
        }

        private void ccSymbols_SelectedItemChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            if (ccSymbols.SelectedSymbol != null)
            {
                UpdateSymbolControls();
            }
            UpdatePreview();
        }

        private void ccSymbols_ListChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void charCharacter_PopupClicked(object sender, EventArgs e)
        {
            ICharacterSymbol cs = ccSymbols.SelectedSymbol as ICharacterSymbol;
            if (cs != null)
            {
                cs.Code = charCharacter.SelectedChar;
            }
            UpdatePreview();
        }

        private void lblPreview_Paint(object sender, PaintEventArgs e)
        {
            UpdatePreview(e.Graphics);
        }

        private void cmbUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_disableUnitWarning) return;
            if (cmbUnits.SelectedItem.ToString() == "World" && _symbolizer.ScaleMode != ScaleMode.Geographic)
            {
                if (MessageBox.Show("Chosing this option will force the drawing to use a Geographic ScaleMode.  Do you wish to continue?", "Change Scale Mode?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    cmbUnits.SelectedItem = _symbolizer.Units.ToString();
                    return;
                }
                _symbolizer.ScaleMode = ScaleMode.Geographic;
            }
            if (cmbUnits.SelectedItem.ToString() != "World" && _symbolizer.ScaleMode == ScaleMode.Geographic)
            {
                if (MessageBox.Show("Chosing this option will force the drawing to use a Symbolic scalemode.  Do you wish to continue?", "Change Scale Mode?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    cmbUnits.SelectedItem = _symbolizer.Units.ToString();
                    return;
                }
                _symbolizer.ScaleMode = ScaleMode.Symbolic;
            }
            GraphicsUnit destination = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), cmbUnits.SelectedItem.ToString());

            GraphicsUnit source = _symbolizer.Units;
            double scale = 1;
            if (source == GraphicsUnit.Inch && destination == GraphicsUnit.Millimeter)
            {
                scale = 25.4;
            }
            if (source == GraphicsUnit.Millimeter && destination == GraphicsUnit.Inch)
            {
                scale = 1 / 25.4;
            }

            _symbolizer.Scale(scale);

            UpdateSymbolControls();
        }

        private void chkSmoothing_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            _symbolizer.Smoothing = chkSmoothing.Checked;
            UpdatePreview();
        }

        private void cmbScaleMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            _symbolizer.ScaleMode = Global.ParseEnum<ScaleMode>(cmbScaleMode.SelectedItem.ToString());
        }

        private void cmbSymbolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSymbolType.SelectedItem.ToString() == "Simple")
            {
                if (tabSymbolProperties.TabPages.Contains(tabPicture))
                {
                    tabSymbolProperties.TabPages.Remove(tabPicture);
                }
                if (tabSymbolProperties.TabPages.Contains(tabCharacter))
                {
                    tabSymbolProperties.TabPages.Remove(tabCharacter);
                }
                if (tabSymbolProperties.TabPages.Contains(tabSimple) == false)
                {
                    tabSymbolProperties.TabPages.Insert(0, tabSimple);
                    tabSymbolProperties.SelectedTab = tabSimple;
                }
            }
            if (cmbSymbolType.SelectedItem.ToString() == "Character")
            {
                if (tabSymbolProperties.TabPages.Contains(tabPicture))
                {
                    tabSymbolProperties.TabPages.Remove(tabPicture);
                }
                if (tabSymbolProperties.TabPages.Contains(tabSimple))
                {
                    tabSymbolProperties.TabPages.Remove(tabSimple);
                }
                if (tabSymbolProperties.TabPages.Contains(tabCharacter) == false)
                {
                    tabSymbolProperties.TabPages.Insert(0, tabCharacter);
                    tabSymbolProperties.SelectedTab = tabCharacter;
                }
            }
            if (cmbSymbolType.SelectedItem.ToString() == "Picture")
            {
                if (tabSymbolProperties.TabPages.Contains(tabSimple))
                {
                    tabSymbolProperties.TabPages.Remove(tabSimple);
                }
                if (tabSymbolProperties.TabPages.Contains(tabCharacter))
                {
                    tabSymbolProperties.TabPages.Remove(tabCharacter);
                }
                if (tabSymbolProperties.TabPages.Contains(tabPicture) == false)
                {
                    tabSymbolProperties.TabPages.Insert(0, tabPicture);
                    tabSymbolProperties.SelectedTab = tabPicture;
                }
            }

            if (_ignoreChanges) return;

            int index = ccSymbols.Symbols.IndexOf(ccSymbols.SelectedSymbol);
            if (index == -1) return;
            ISymbol oldSymbol = ccSymbols.SelectedSymbol;

            if (cmbSymbolType.SelectedItem.ToString() == "Simple")
            {
                SimpleSymbol ss = new SimpleSymbol();
                if (oldSymbol != null) ss.CopyPlacement(oldSymbol);
                ccSymbols.Symbols[index] = ss;
                ccSymbols.RefreshList();
                ccSymbols.SelectedSymbol = ss;
            }
            if (cmbSymbolType.SelectedItem.ToString() == "Character")
            {
                CharacterSymbol cs = new CharacterSymbol();
                if (oldSymbol != null) cs.CopyPlacement(oldSymbol);
                ccSymbols.Symbols[index] = cs;
                ccSymbols.RefreshList();
                ccSymbols.SelectedSymbol = cs;
            }
            if (cmbSymbolType.SelectedItem.ToString() == "Picture")
            {
                PictureSymbol ps = new PictureSymbol();
                if (oldSymbol != null) ps.CopyPlacement(oldSymbol);
                ccSymbols.Symbols[index] = ps;
                ccSymbols.RefreshList();
                ccSymbols.SelectedSymbol = ps;
            }
        }

        private void cmbPointShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            ISimpleSymbol ss = ccSymbols.SelectedSymbol as ISimpleSymbol;
            if (ss == null) return;
            ss.PointShape = Global.ParseEnum<PointShape>(cmbPointShape.SelectedItem.ToString());
            UpdatePreview();
        }

        private void cbColorSimple_ColorChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IColorable c = ccSymbols.SelectedSymbol as IColorable;
            if (c != null)
            {
                c.Color = cbColorSimple.Color;
                sldOpacitySimple.MaximumColor = Color.FromArgb(255, c.Color);
                sldOpacitySimple.Value = c.Opacity;
                sldOpacitySimple.Invalidate();
            }
            UpdatePreview();
        }

        private void sldOpacitySimple_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IColorable c = ccSymbols.SelectedSymbol as IColorable;
            if (c != null)
            {
                c.Opacity = (float)sldOpacitySimple.Value;
                cbColorSimple.Color = c.Color;
            }
            UpdatePreview();
        }

        private void chkUseOutline_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.UseOutline = chkUseOutline.Checked;
            }
            UpdatePreview();
        }

        private void sizeControlSimple_SelectedSizeChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            UpdatePreview();
        }

        private void dbxOutlineWidth_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.OutlineWidth = dbxOutlineWidth.Value;
            }
            UpdatePreview();
        }

        private void cbOutlineColor_ColorChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.OutlineColor = cbOutlineColor.Color;
                sldOutlineOpacity.MaximumColor = Color.FromArgb(255, cbOutlineColor.Color);
                sldOutlineOpacity.Value = os.OutlineOpacity;
                sldOutlineOpacity.Refresh();
            }
            UpdatePreview();
        }

        private void sldOutlineOpacity_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.OutlineOpacity = (float)sldOutlineOpacity.Value;
                cbOutlineColor.Color = os.OutlineColor;
            }
            UpdatePreview();
        }

        private void angleControlSimple_AngleChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            ISymbol s = ccSymbols.SelectedSymbol;
            if (s != null)
            {
                s.Angle = -angleControl.Angle;
            }
            UpdatePreview();
        }

        private void cbColorCharacter_ColorChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IColorable c = ccSymbols.SelectedSymbol as IColorable;
            if (c != null)
            {
                c.Color = cbColorCharacter.Color;
                sldOpacityCharacter.MaximumColor = Color.FromArgb(255, c.Color);
                sldOpacityCharacter.Value = c.Opacity;
            }
            UpdatePreview();
        }

        private void sldOpacityCharacter_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IColorable c = ccSymbols.SelectedSymbol as IColorable;
            if (c != null)
            {
                c.Opacity = (float)sldOpacityCharacter.Value;
                cbColorCharacter.Color = c.Color;
            }
            UpdatePreview();
        }

        private void cmbFontFamilly_SelectedItemChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            ICharacterSymbol cs = ccSymbols.SelectedSymbol as ICharacterSymbol;
            if (cs != null)
            {
                cs.FontFamilyName = cmbFontFamilly.SelectedFamily;
                charCharacter.Font = new Font(cs.FontFamilyName, 10, cs.Style);
                // charCharacter.
            }
            UpdatePreview();
        }

        private void dbxOffsetXSimple_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            ISymbol s = ccSymbols.SelectedSymbol;
            if (s != null)
            {
                s.Offset.X = dbxOffsetX.Value;
                UpdatePreview();
            }
        }

        private void dbxOffsetYSimple_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            ISymbol s = ccSymbols.SelectedSymbol;
            if (s != null)
            {
                s.Offset.Y = dbxOffsetY.Value;
                UpdatePreview();
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.bmp;*.jpg;*.gif;*.tif;*.png;*.ico";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            IPictureSymbol ps = ccSymbols.SelectedSymbol as IPictureSymbol;
            if (ps != null)
            {
                ps.ImageFilename = ofd.FileName;
                txtImageFilename.Text = Path.GetFileName(ofd.FileName);
            }
            UpdatePreview();
        }

        private void chkUseOutlinePicture_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.UseOutline = chkUseOutlinePicture.Checked;
                UpdatePreview();
            }
        }

        private void dbxOutlineWidthPicture_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.OutlineWidth = dbxOutlineWidthPicture.Value;
                UpdatePreview();
            }
        }

        private void cbOutlineColorPicture_ColorChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.OutlineColor = cbOutlineColorPicture.Color;
                sldOutlineOpacityPicture.MaximumColor = Color.FromArgb(255, os.OutlineColor);
                sldOutlineOpacityPicture.Value = os.OutlineOpacity;
                sldOutlineOpacityPicture.Invalidate();
                UpdatePreview();
            }
        }

        private void sldOutlineOpacityPicture_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IOutlinedSymbol os = ccSymbols.SelectedSymbol as IOutlinedSymbol;
            if (os != null)
            {
                os.OutlineOpacity = (float)sldOutlineOpacityPicture.Value;
                cbOutlineColorPicture.Color = os.OutlineColor;
                UpdatePreview();
            }
        }

        private void sldImageOpacity_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;
            IPictureSymbol ps = ccSymbols.SelectedSymbol as IPictureSymbol;
            if (ps != null)
            {
                ps.Opacity = (float)sldImageOpacity.Value;
                UpdatePreview();
            }
        }

        private void btnAddToCustom_Click(object sender, EventArgs e)
        {
            OnAddToCustomSymbols();
        }

        #endregion

        #endregion

        #region Private Functions

        private void UpdateSymbolControls()
        {
            _ignoreChanges = true;
            cmbScaleMode.SelectedItem = _symbolizer.ScaleMode.ToString();
            chkSmoothing.Checked = _symbolizer.Smoothing;
            _disableUnitWarning = true;
            cmbUnits.SelectedItem = _symbolizer.Units.ToString();
            _disableUnitWarning = false;
            ICharacterSymbol cs = ccSymbols.SelectedSymbol as ICharacterSymbol;
            if (cs != null)
            {
                cmbSymbolType.SelectedItem = "Character";
                UpdateCharacterSymbolControls(cs);
            }
            ISimpleSymbol ss = ccSymbols.SelectedSymbol as ISimpleSymbol;
            if (ss != null)
            {
                cmbSymbolType.SelectedItem = "Simple";
                UpdateSimpleSymbolControls(ss);
            }
            IPictureSymbol ps = ccSymbols.SelectedSymbol as IPictureSymbol;
            if (ps != null)
            {
                cmbSymbolType.SelectedItem = "Picture";
                UpdatePictureSymbolControls(ps);
            }
            _ignoreChanges = false;
        }

        private void UpdateCharacterSymbolControls(ICharacterSymbol cs)
        {
            cmbFontFamilly.SelectedFamily = cs.FontFamilyName;
            txtUnicode.Text = ((int)cs.Character).ToString();
            charCharacter.TypeSet = (byte)(cs.Character / 256);
            charCharacter.SelectedChar = (byte)(cs.Character % 256);
            charCharacter.Font = new Font(cs.FontFamilyName, 10, cs.Style);
            cbColorCharacter.Color = cs.Color;
            sldOpacityCharacter.Value = ((double)cs.Color.A / 255);
            sldOpacityCharacter.MaximumColor = Color.FromArgb(255, cs.Color);

            angleControl.Angle = -(int)cs.Angle;
            dbxOffsetX.Value = cs.Offset.X;
            dbxOffsetY.Value = cs.Offset.Y;
            sizeControl.Symbol = cs;
        }

        private void UpdateSimpleSymbolControls(ISimpleSymbol ss)
        {
            angleControl.Angle = -(int)ss.Angle;
            dbxOffsetX.Value = ss.Offset.X;
            dbxOffsetY.Value = ss.Offset.Y;
            sizeControl.Symbol = ss;

            cmbPointShape.SelectedItem = ss.PointShape.ToString();
            cbColorSimple.Color = ss.Color;
            sldOpacitySimple.Value = ss.Opacity;
            sldOpacitySimple.MaximumColor = Color.FromArgb(255, ss.Color);
            chkUseOutline.Checked = ss.UseOutline;
            dbxOutlineWidth.Value = ss.OutlineWidth;
            cbOutlineColor.Color = ss.OutlineColor;
            sldOutlineOpacity.Value = ss.Opacity;
            sldOutlineOpacity.MaximumColor = Color.FromArgb(255, ss.OutlineColor);
        }

        private void UpdatePictureSymbolControls(IPictureSymbol ps)
        {
            angleControl.Angle = -(int)ps.Angle;
            dbxOffsetX.Value = ps.Offset.X;
            dbxOffsetY.Value = ps.Offset.Y;
            sizeControl.Symbol = ps;

            sldImageOpacity.Value = ps.Opacity;
            txtImageFilename.Text = Path.GetFileName(ps.ImageFilename);

            chkUseOutlinePicture.Checked = ps.UseOutline;
            cbOutlineColorPicture.Color = ps.OutlineColor;
            sldOutlineOpacityPicture.Value = ps.OutlineOpacity;
            sldOutlineOpacityPicture.MaximumColor = Color.FromArgb(255, ps.OutlineColor);
            dbxOutlineWidth.Value = ps.OutlineWidth;
        }

        private void UpdatePreview(Graphics g)
        {
            if (_symbolizer == null) return;
            g.Clear(Color.White);
            Matrix shift = g.Transform;
            shift.Translate((float)lblPreview.Width / 2, (float)lblPreview.Height / 2);
            g.Transform = shift;
            double scale = 1;
            Size2D symbolSize = _symbolizer.GetSize();
            if (_symbolizer.ScaleMode == ScaleMode.Geographic || symbolSize.Height > (lblPreview.Height - 6))
            {
                scale = (lblPreview.Height - 6) / symbolSize.Height;
            }
            _symbolizer.Draw(g, scale);
        }

        private void UpdatePreview()
        {
            ccSymbols.Refresh();
            sizeControl.Refresh();
            Graphics g = lblPreview.CreateGraphics();
            UpdatePreview(g);
            g.Dispose();
        }

        #endregion
    }
}