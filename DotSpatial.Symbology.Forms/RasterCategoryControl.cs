// ********************************************************************************************************
// Product Name: Components.CategoriesViewer.dll Alpha
// Description:  The basic module for CategoriesViewer version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from CategoriesViewer.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/14/2009 4:13:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Dialog for the 'unique values' feature symbol classification scheme
    /// </summary>
    public class RasterCategoryControl : UserControl, ICategoryControl
    {
        #region Events

        /// <summary>
        /// Occurs when the apply changes option has been triggered.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Private Variables

        //the original scheme which is modified only after clicking 'Apply'
        private int _activeCategoryIndex;
        private Timer _cleanupTimer;
        private int _dblClickEditIndex;
        private ContextMenu _elevationQuickPick;
        private bool _ignoreEnter;

        //the attribute data Table
        private bool _ignoreRefresh;
        private bool _ignoreValidation;
        private double _maximum;
        private double _minimum;
        private IRasterLayer _newLayer;
        private IColorScheme _newScheme;
        private IColorScheme _originalScheme;
        private IRasterLayer _originalLayer;
        private ContextMenu _quickSchemes;
        private IRaster _raster;
        private PropertyDialog _shadedReliefDialog;
        private IRasterSymbolizer _symbolizer;
        private TabColorDialog _tabColorDialog;
        private AngleControl angLightDirection;
        private BreakSliderGraph breakSliderGraph1;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnElevation;
        private Button btnQuick;
        private Button btnRamp;
        private Button btnShadedRelief;
        private CheckBox chkHillshade;
        private CheckBox chkLog;
        private CheckBox chkShowMean;
        private CheckBox chkShowStd;
        private ComboBox cmbInterval;
        private ComboBox cmbIntervalSnapping;
        private Button cmdRefresh;
        private DataGridViewTextBoxColumn colCount;
        private DataGridViewTextBoxColumn colLegendText;
        private DataGridViewImageColumn colSymbol;
        private DataGridViewTextBoxColumn colValues;
        private ColorButton colorNoData;
        private IContainer components = null;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DoubleBox dbxElevationFactor;
        private DoubleBox dbxMax;
        private DoubleBox dbxMin;
        private DataGridView dgvCategories;
        private DataGridView dgvStatistics;
        private GroupBox groupBox1;
        private GroupBox grpHillshade;
        private Label label1;
        private Label label2;
        private Label lblBreaks;
        private Label lblColumns;
        private Label lblSigFig;
        private SymbologyProgressBar mwProgressBar1;
        private NumericUpDown nudCategoryCount;
        private NumericUpDown nudColumns;
        private NumericUpDown nudSigFig;
        private RampSlider opacityNoData;
        private RampSlider sldSchemeOpacity;
        private DataGridViewTextBoxColumn stat;
        private TabPage tabGraph;
        private TabControl tabScheme;
        private TabPage tabStatistics;
        private TabColorControl tccColorRange;
        private ToolTip ttHelp;
        private DataGridViewTextBoxColumn Stat;
        private DataGridViewTextBoxColumn Value;
        private DataGridViewTextBoxColumn value;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an empty FeatureCategoryControl without specifying any particular layer to use
        /// </summary>
        public RasterCategoryControl()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new instance of the unique values category Table
        /// </summary>
        /// <param name="layer">The feature set that is used</param>
        public RasterCategoryControl(IRasterLayer layer)
        {
            InitializeComponent();
            Configure();
            Initialize(layer);
        }

        /// <summary>
        /// Handles the mouse wheel, allowing the breakSldierGraph to zoom in or out.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Point screenLoc = PointToScreen(e.Location);
            Point bsPoint = breakSliderGraph1.PointToClient(screenLoc);
            if (breakSliderGraph1.ClientRectangle.Contains(bsPoint))
            {
                breakSliderGraph1.DoMouseWheel(e.Delta, bsPoint.X);
                return;
            }
            base.OnMouseWheel(e);
        }

        private void Configure()
        {
            _elevationQuickPick = new ContextMenu();

            _elevationQuickPick.MenuItems.Add("Z Feet     | XY Lat Long", SetElevationFeetLatLong);
            _elevationQuickPick.MenuItems.Add("Z Feet     | XY Meters", SetElevationFeetMeters);
            _elevationQuickPick.MenuItems.Add("Z Feet     | XY Feet", SetElevationSameUnits);
            _elevationQuickPick.MenuItems.Add("Z Meters | XY Lat Long", SetElevationMetersLatLong);
            _elevationQuickPick.MenuItems.Add("Z Meters | XY Meters", SetElevationSameUnits);
            _elevationQuickPick.MenuItems.Add("Z Meters | XY Feet", SetElevationMetersFeet);
            dgvCategories.CellFormatting += DgvCategoriesCellFormatting;
            dgvCategories.CellDoubleClick += DgvCategoriesCellDoubleClick;
            dgvCategories.SelectionChanged += DgvCategoriesSelectionChanged;
            dgvCategories.CellValidated += DgvCategoriesCellValidated;
            dgvCategories.MouseDown += DgvCategoriesMouseDown;
            cmbInterval.SelectedItem = "EqualInterval";
            breakSliderGraph1.SliderSelected += BreakSliderGraph1SliderSelected;
            _quickSchemes = new ContextMenu();
            string[] names = Enum.GetNames(typeof(ColorSchemeType));
            foreach (string name in names)
            {
                MenuItem mi = new MenuItem(name, QuickSchemeClicked);
                _quickSchemes.MenuItems.Add(mi);
            }
            cmbIntervalSnapping.Items.Clear();
            var result = Enum.GetValues(typeof(IntervalSnapMethod));
            foreach (var item in result)
            {
                cmbIntervalSnapping.Items.Add(item);
            }
            cmbIntervalSnapping.SelectedItem = IntervalSnapMethod.Rounding;
            _cleanupTimer = new Timer();
            _cleanupTimer.Interval = 10;
            _cleanupTimer.Tick += CleanupTimerTick;

            // Allows shaded Relief to be edited
            _shadedReliefDialog = new PropertyDialog();
            _shadedReliefDialog.ChangesApplied += PropertyDialogChangesApplied;
        }

        private void SetElevationFeetMeters(object sender, EventArgs e)
        {
            dbxElevationFactor.Value = .3048;
        }

        private void SetElevationSameUnits(object sender, EventArgs e)
        {
            dbxElevationFactor.Value = 1;
        }

        private void SetElevationMetersFeet(object sender, EventArgs e)
        {
            dbxElevationFactor.Value = 3.2808399;
        }

        private void SetElevationFeetLatLong(object sender, EventArgs e)
        {
            dbxElevationFactor.Value = 1 / (111319.9 * 3.2808399);
        }

        private void SetElevationMetersLatLong(object sender, EventArgs e)
        {
            dbxElevationFactor.Value = 1 / 111319.9;
        }

        private void DgvCategoriesMouseDown(object sender, MouseEventArgs e)
        {
            if (_ignoreEnter) return;
            _activeCategoryIndex = dgvCategories.HitTest(e.X, e.Y).RowIndex;
        }

        private void CleanupTimerTick(object sender, EventArgs e)
        {
            // When a row validation causes rows above the edit row to be removed,
            // we can't easily update the Table during the validation event.
            // The timer allows the validation to finish before updating the Table.
            _cleanupTimer.Stop();
            _ignoreValidation = true;
            UpdateTable();
            if (_activeCategoryIndex >= 0 && _activeCategoryIndex < dgvCategories.Rows.Count)
            {
                dgvCategories.Rows[_activeCategoryIndex].Selected = true;
            }

            _ignoreValidation = false;
            _ignoreEnter = false;
        }

        private void BreakSliderGraph1SliderSelected(object sender, BreakSliderEventArgs e)
        {
            int index = breakSliderGraph1.Breaks.IndexOf(e.Slider);
            dgvCategories.Rows[index].Selected = true;
        }

        private void DgvCategoriesCellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (_ignoreValidation) return;
            if (_newScheme.Categories.Count <= e.RowIndex) return;

            if (e.ColumnIndex == 2)
            {
                IColorCategory fctxt = _newScheme.Categories[e.RowIndex];
                fctxt.LegendText = (string)dgvCategories[e.ColumnIndex, e.RowIndex].Value;
                return;
            }

            if (e.ColumnIndex != 1) return;

            IColorCategory cb = _newScheme.Categories[e.RowIndex];
            if ((string)dgvCategories[e.ColumnIndex, e.RowIndex].Value == cb.LegendText) return;
            _ignoreEnter = true;
            string exp = (string)dgvCategories[e.ColumnIndex, e.RowIndex].Value;
            
            cb.LegendText = exp;

            cb.Range = new Range(exp);
            if (cb.Range.Maximum != null && cb.Range.Maximum > _raster.Maximum)
            {
                cb.Range.Maximum = _raster.Maximum;
            }
            if (cb.Range.Minimum != null && cb.Range.Minimum > _raster.Maximum)
            {
                cb.Range.Minimum = _raster.Maximum;
            }
            if (cb.Range.Maximum != null && cb.Range.Minimum < _raster.Minimum)
            {
                cb.Range.Minimum = _raster.Minimum;
            }
            if (cb.Range.Minimum != null && cb.Range.Minimum < _raster.Minimum)
            {
                cb.Range.Minimum = _raster.Minimum;
            }
            cb.ApplyMinMax(_newScheme.EditorSettings);
            ColorCategoryCollection breaks = _newScheme.Categories;
            breaks.SuspendEvents();
            if (cb.Range.Minimum == null && cb.Range.Maximum == null)
            {
                breaks.Clear();
                breaks.Add(cb);
            }
            else if (cb.Range.Maximum == null)
            {
                List<IColorCategory> removeList = new List<IColorCategory>();

                int iPrev = e.RowIndex - 1;
                for (int i = 0; i < e.RowIndex; i++)
                {
                    // If the specified max is below the minima of a lower range, remove the lower range.
                    if (breaks[i].Minimum > cb.Minimum)
                    {
                        removeList.Add(breaks[i]);
                        iPrev--;
                    }
                    else if (breaks[i].Maximum > cb.Minimum || i == iPrev)
                    {
                        // otherwise, if the maximum of a lower range is higher than the value, adjust it.
                        breaks[i].Maximum = cb.Minimum;
                        breaks[i].ApplyMinMax(_symbolizer.EditorSettings);
                    }
                }
                for (int i = e.RowIndex + 1; i < breaks.Count(); i++)
                {
                    // Since we have just assigned an absolute maximum, any previous categories
                    // that fell above the edited category should be removed.
                    removeList.Add(breaks[i]);
                }
                foreach (IColorCategory brk in removeList)
                {
                    // Do the actual removal.
                    breaks.Remove(brk);
                }
            }
            else if (cb.Range.Minimum == null)
            {
                List<IColorCategory> removeList = new List<IColorCategory>();

                int iNext = e.RowIndex + 1;
                for (int i = e.RowIndex + 1; i < breaks.Count; i++)
                {
                    // If the specified max is below the minima of a lower range, remove the lower range.
                    if (breaks[i].Maximum < cb.Maximum)
                    {
                        removeList.Add(breaks[i]);
                        iNext++;
                    }
                    else if (breaks[i].Minimum < cb.Maximum || i == iNext)
                    {
                        // otherwise, if the maximum of a lower range is higher than the value, adjust it.
                        breaks[i].Minimum = cb.Maximum;
                        breaks[i].ApplyMinMax(_symbolizer.EditorSettings);
                    }
                }
                for (int i = 0; i < e.RowIndex; i++)
                {
                    // Since we have just assigned an absolute minimum, any previous categories
                    // that fell above the edited category should be removed.
                    removeList.Add(breaks[i]);
                }

                foreach (IColorCategory brk in removeList)
                {
                    // Do the actual removal.
                    breaks.Remove(brk);
                }
            }
            else
            {
                // We have two values.  Adjust any above or below that conflict.
                List<IColorCategory> removeList = new List<IColorCategory>();
                int iPrev = e.RowIndex - 1;
                for (int i = 0; i < e.RowIndex; i++)
                {
                    // If the specified max is below the minima of a lower range, remove the lower range.
                    if (breaks[i].Minimum > cb.Minimum)
                    {
                        removeList.Add(breaks[i]);
                        iPrev--;
                    }
                    else if (breaks[i].Maximum > cb.Minimum || i == iPrev)
                    {
                        // otherwise, if the maximum of a lower range is higher than the value, adjust it.
                        breaks[i].Maximum = cb.Minimum;
                        breaks[i].ApplyMinMax(_symbolizer.EditorSettings);
                    }
                }
                int iNext = e.RowIndex + 1;
                for (int i = e.RowIndex + 1; i < breaks.Count; i++)
                {
                    // If the specified max is below the minima of a lower range, remove the lower range.
                    if (breaks[i].Maximum < cb.Maximum)
                    {
                        removeList.Add(breaks[i]);
                        iNext++;
                    }
                    else if (breaks[i].Minimum < cb.Maximum || i == iNext)
                    {
                        // otherwise, if the maximum of a lower range is higher than the value, adjust it.
                        breaks[i].Minimum = cb.Maximum;
                        breaks[i].ApplyMinMax(_symbolizer.EditorSettings);
                    }
                }
                foreach (IColorCategory brk in removeList)
                {
                    // Do the actual removal.
                    breaks.Remove(brk);
                }
            }
            breaks.ResumeEvents();
            _ignoreRefresh = true;
            cmbInterval.SelectedItem = IntervalMethod.Manual.ToString();
            _symbolizer.EditorSettings.IntervalMethod = IntervalMethod.Manual;
            _ignoreRefresh = false;
            UpdateStatistics(false);
            _cleanupTimer.Start();
        }

        private void DgvCategoriesSelectionChanged(object sender, EventArgs e)
        {
            if (breakSliderGraph1 == null) return;
            if (breakSliderGraph1.Breaks == null) return;
            if (dgvCategories.SelectedRows.Count > 0)
            {
                int index = dgvCategories.Rows.IndexOf(dgvCategories.SelectedRows[0]);
                if (breakSliderGraph1.Breaks.Count == 0 || index >= breakSliderGraph1.Breaks.Count) return;
                breakSliderGraph1.SelectBreak(breakSliderGraph1.Breaks[index]);
            }
            else
            {
                breakSliderGraph1.SelectBreak(null);
            }
            breakSliderGraph1.Invalidate();
        }

        /// <summary>
        /// Sets up the Table to work with the specified layer
        /// </summary>
        /// <param name="layer"></param>
        public void Initialize(IRasterLayer layer)
        {
            _originalLayer = layer;
            _newLayer = layer.Copy();
            _symbolizer = layer.Symbolizer;
            _newScheme = _symbolizer.Scheme;
            _originalScheme = (IColorScheme)_symbolizer.Scheme.Clone();
            _raster = layer.DataSet;
            GetSettings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the Table using the unique values
        /// </summary>
        private void UpdateTable()
        {
            dgvCategories.SuspendLayout();
            dgvCategories.Rows.Clear();

            ColorCategoryCollection breaks = _newScheme.Categories;
            int i = 0;
            if (breaks.Count > 0)
            {
                dgvCategories.Rows.Add(breaks.Count);
                foreach (IColorCategory brk in breaks)
                {
                    dgvCategories[1, i].Value = brk.Range.ToString(_symbolizer.EditorSettings.IntervalSnapMethod,
                                                                   _symbolizer.EditorSettings.IntervalRoundingDigits);
                    dgvCategories[2, i].Value = brk.LegendText;
                    i++;
                }
            }
            dgvCategories.ResumeLayout();
            dgvCategories.Invalidate();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current progress bar.
        /// </summary>
        public SymbologyProgressBar ProgressBar
        {
            get { return mwProgressBar1; }
        }

        /// <summary>
        /// Gets or sets the Maximum value currently displayed in the graph.
        /// </summary>
        public double Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// Gets or sets the Minimum value currently displayed in the graph.
        /// </summary>
        public double Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Fires the apply changes situation externally, forcing the Table to
        /// write its values to the original layer.
        /// </summary>
        public void ApplyChanges()
        {
            OnApplyChanges();
        }

        /// <summary>
        /// Applies the changes that have been specified in this control
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            // SetSettings(); When applying a scheme settings are set, so don't bother here.
            _originalLayer.Symbolizer = _newLayer.Symbolizer.Copy();
            _originalScheme = _newLayer.Symbolizer.Scheme.Copy();
            //_originalLayer.Symbolizer.Scheme.Categories.UpdateItemParentPointers();
            if (_originalLayer.Symbolizer.ShadedRelief.IsUsed)
            {
                if (_originalLayer.Symbolizer.ShadedRelief.HasChanged || _originalLayer.Symbolizer.HillShade == null)
                    _originalLayer.Symbolizer.CreateHillShade(mwProgressBar1);
            }
            _originalLayer.WriteBitmap(mwProgressBar1);
            if (ChangesApplied != null) ChangesApplied(_originalLayer, EventArgs.Empty);
        }

        /// <summary>
        /// Cancel the action.
        /// </summary>
        public void Cancel()
        {
            OnCancel();
        }

        /// <summary>
        /// Event that fires when the action is canceled.
        /// </summary>
        protected virtual void OnCancel()
        {
            _originalLayer.Symbolizer.Scheme = _originalScheme;
        }

        private void RefreshValues()
        {
            if (_ignoreRefresh) return;
            SetSettings();
            _newScheme.CreateCategories(_raster);
            UpdateTable();
            UpdateStatistics(false); // if the parameter is true, even on manual, the breaks are reset.
            breakSliderGraph1.Invalidate();
        }

        /// <summary>
        /// When the user double clicks the cell then we should display the detailed
        /// symbology dialog
        /// </summary>
        private void DgvCategoriesCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int count = _newScheme.Categories.Count;
            if (e.ColumnIndex == 0 && e.RowIndex < count)
            {
                _dblClickEditIndex = e.RowIndex;
                _tabColorDialog = new TabColorDialog();
                _tabColorDialog.ChangesApplied += TabColorDialogChangesApplied;
                _tabColorDialog.StartColor = _newScheme.Categories[_dblClickEditIndex].LowColor;
                _tabColorDialog.EndColor = _newScheme.Categories[_dblClickEditIndex].HighColor;
                _tabColorDialog.Show(ParentForm);
            }
        }

        private void TabColorDialogChangesApplied(object sender, EventArgs e)
        {
            if (_newScheme == null) return;
            if (_newScheme.Categories == null) return;
            if (_dblClickEditIndex < 0 || _dblClickEditIndex > _newScheme.Categories.Count) return;
            _newScheme.Categories[_dblClickEditIndex].LowColor = _tabColorDialog.StartColor;
            _newScheme.Categories[_dblClickEditIndex].HighColor = _tabColorDialog.EndColor;
            UpdateTable();
        }

        /// <summary>
        /// When the cell is formatted
        /// </summary>
        private void DgvCategoriesCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_newScheme == null) return;
            int count = _newScheme.Categories.Count;
            if (count == 0) return;

            // Replace string values in the column with images.
            if (e.ColumnIndex != 0) return;
            Image img = e.Value as Image;
            if (img == null)
            {
                img = SymbologyFormsImages.info;
                e.Value = img;
            }
            Graphics g = Graphics.FromImage(img);
            g.Clear(Color.White);

            if (count > e.RowIndex)
            {
                Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
                _newScheme.DrawCategory(e.RowIndex, g, rect);
            }
            g.Dispose();
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterCategoryControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.btnQuick = new System.Windows.Forms.Button();
            this.btnRamp = new System.Windows.Forms.Button();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.dbxElevationFactor = new DotSpatial.Symbology.Forms.DoubleBox();
            this.angLightDirection = new DotSpatial.Symbology.Forms.AngleControl();
            this.tabScheme = new System.Windows.Forms.TabControl();
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.dbxMax = new DotSpatial.Symbology.Forms.DoubleBox();
            this.dbxMin = new DotSpatial.Symbology.Forms.DoubleBox();
            this.nudSigFig = new System.Windows.Forms.NumericUpDown();
            this.lblSigFig = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbIntervalSnapping = new System.Windows.Forms.ComboBox();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();
            this.Stat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbInterval = new System.Windows.Forms.ComboBox();
            this.nudCategoryCount = new System.Windows.Forms.NumericUpDown();
            this.lblBreaks = new System.Windows.Forms.Label();
            this.tabGraph = new System.Windows.Forms.TabPage();
            this.lblColumns = new System.Windows.Forms.Label();
            this.nudColumns = new System.Windows.Forms.NumericUpDown();
            this.chkLog = new System.Windows.Forms.CheckBox();
            this.chkShowStd = new System.Windows.Forms.CheckBox();
            this.chkShowMean = new System.Windows.Forms.CheckBox();
            this.breakSliderGraph1 = new DotSpatial.Symbology.Forms.BreakSliderGraph();
            this.dgvCategories = new System.Windows.Forms.DataGridView();
            this.colSymbol = new System.Windows.Forms.DataGridViewImageColumn();
            this.colValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLegendText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkHillshade = new System.Windows.Forms.CheckBox();
            this.btnShadedRelief = new System.Windows.Forms.Button();
            this.grpHillshade = new System.Windows.Forms.GroupBox();
            this.btnElevation = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.opacityNoData = new DotSpatial.Symbology.Forms.RampSlider();
            this.colorNoData = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldSchemeOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.mwProgressBar1 = new DotSpatial.Symbology.Forms.SymbologyProgressBar();
            this.tccColorRange = new DotSpatial.Symbology.Forms.TabColorControl();
            this.tabScheme.SuspendLayout();
            this.tabStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigFig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoryCount)).BeginInit();
            this.tabGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            this.grpHillshade.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuick
            // 
            resources.ApplyResources(this.btnQuick, "btnQuick");
            this.btnQuick.Name = "btnQuick";
            this.ttHelp.SetToolTip(this.btnQuick, resources.GetString("btnQuick.ToolTip"));
            this.btnQuick.UseVisualStyleBackColor = true;
            this.btnQuick.Click += new System.EventHandler(this.btnQuick_Click);
            // 
            // btnRamp
            // 
            resources.ApplyResources(this.btnRamp, "btnRamp");
            this.btnRamp.Name = "btnRamp";
            this.ttHelp.SetToolTip(this.btnRamp, resources.GetString("btnRamp.ToolTip"));
            this.btnRamp.UseVisualStyleBackColor = true;
            this.btnRamp.Click += new System.EventHandler(this.btnRamp_Click);
            // 
            // cmdRefresh
            // 
            resources.ApplyResources(this.cmdRefresh, "cmdRefresh");
            this.cmdRefresh.Name = "cmdRefresh";
            this.ttHelp.SetToolTip(this.cmdRefresh, resources.GetString("cmdRefresh.ToolTip"));
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // dbxElevationFactor
            // 
            this.dbxElevationFactor.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxElevationFactor.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxElevationFactor, "dbxElevationFactor");
            this.dbxElevationFactor.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxElevationFactor.IsValid = true;
            this.dbxElevationFactor.Name = "dbxElevationFactor";
            this.dbxElevationFactor.NumberFormat = "E7";
            this.dbxElevationFactor.RegularHelp = "The Elevation Factor is a constant multiplier that converts the elevation unit (e" +
    "g. feet) into the projection units (eg. decimal degrees).";
            this.ttHelp.SetToolTip(this.dbxElevationFactor, resources.GetString("dbxElevationFactor.ToolTip"));
            this.dbxElevationFactor.Value = 0D;
            this.dbxElevationFactor.TextChanged += new System.EventHandler(this.dbxElevationFactor_TextChanged);
            // 
            // angLightDirection
            // 
            this.angLightDirection.Angle = 45;
            this.angLightDirection.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.angLightDirection, "angLightDirection");
            this.angLightDirection.Clockwise = true;
            this.angLightDirection.KnobColor = System.Drawing.Color.Green;
            this.angLightDirection.Name = "angLightDirection";
            this.angLightDirection.StartAngle = 90;
            this.ttHelp.SetToolTip(this.angLightDirection, resources.GetString("angLightDirection.ToolTip"));
            this.angLightDirection.AngleChanged += new System.EventHandler(this.angLightDirection_AngleChanged);
            // 
            // tabScheme
            // 
            this.tabScheme.Controls.Add(this.tabStatistics);
            this.tabScheme.Controls.Add(this.tabGraph);
            resources.ApplyResources(this.tabScheme, "tabScheme");
            this.tabScheme.Name = "tabScheme";
            this.tabScheme.SelectedIndex = 0;
            // 
            // tabStatistics
            // 
            this.tabStatistics.Controls.Add(this.dbxMax);
            this.tabStatistics.Controls.Add(this.dbxMin);
            this.tabStatistics.Controls.Add(this.nudSigFig);
            this.tabStatistics.Controls.Add(this.lblSigFig);
            this.tabStatistics.Controls.Add(this.label1);
            this.tabStatistics.Controls.Add(this.cmbIntervalSnapping);
            this.tabStatistics.Controls.Add(this.dgvStatistics);
            this.tabStatistics.Controls.Add(this.label2);
            this.tabStatistics.Controls.Add(this.cmbInterval);
            this.tabStatistics.Controls.Add(this.nudCategoryCount);
            this.tabStatistics.Controls.Add(this.lblBreaks);
            resources.ApplyResources(this.tabStatistics, "tabStatistics");
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.UseVisualStyleBackColor = true;
            // 
            // dbxMax
            // 
            this.dbxMax.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMax.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMax, "dbxMax");
            this.dbxMax.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMax.IsValid = true;
            this.dbxMax.Name = "dbxMax";
            this.dbxMax.NumberFormat = null;
            this.dbxMax.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMax.Value = 1000000D;
            this.dbxMax.TextChanged += new System.EventHandler(this.dbxMax_TextChanged);
            // 
            // dbxMin
            // 
            this.dbxMin.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMin.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMin, "dbxMin");
            this.dbxMin.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMin.IsValid = true;
            this.dbxMin.Name = "dbxMin";
            this.dbxMin.NumberFormat = null;
            this.dbxMin.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMin.Value = -100000D;
            this.dbxMin.TextChanged += new System.EventHandler(this.dbxMin_TextChanged);
            // 
            // nudSigFig
            // 
            resources.ApplyResources(this.nudSigFig, "nudSigFig");
            this.nudSigFig.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudSigFig.Name = "nudSigFig";
            this.nudSigFig.ValueChanged += new System.EventHandler(this.nudSigFig_ValueChanged);
            // 
            // lblSigFig
            // 
            resources.ApplyResources(this.lblSigFig, "lblSigFig");
            this.lblSigFig.Name = "lblSigFig";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbIntervalSnapping
            // 
            this.cmbIntervalSnapping.FormattingEnabled = true;
            resources.ApplyResources(this.cmbIntervalSnapping, "cmbIntervalSnapping");
            this.cmbIntervalSnapping.Name = "cmbIntervalSnapping";
            this.cmbIntervalSnapping.SelectedIndexChanged += new System.EventHandler(this.cmbIntervalSnapping_SelectedIndexChanged);
            // 
            // dgvStatistics
            // 
            this.dgvStatistics.AllowUserToAddRows = false;
            this.dgvStatistics.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.AntiqueWhite;
            this.dgvStatistics.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvStatistics.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStatistics.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistics.ColumnHeadersVisible = false;
            this.dgvStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Stat,
            this.Value});
            resources.ApplyResources(this.dgvStatistics, "dgvStatistics");
            this.dgvStatistics.Name = "dgvStatistics";
            this.dgvStatistics.RowHeadersVisible = false;
            this.dgvStatistics.ShowCellErrors = false;
            // 
            // Stat
            // 
            resources.ApplyResources(this.Stat, "Stat");
            this.Stat.Name = "Stat";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Value, "Value");
            this.Value.Name = "Value";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbInterval
            // 
            this.cmbInterval.FormattingEnabled = true;
            this.cmbInterval.Items.AddRange(new object[] {
            resources.GetString("cmbInterval.Items"),
            resources.GetString("cmbInterval.Items1"),
            resources.GetString("cmbInterval.Items2")});
            resources.ApplyResources(this.cmbInterval, "cmbInterval");
            this.cmbInterval.Name = "cmbInterval";
            this.cmbInterval.SelectedIndexChanged += new System.EventHandler(this.cmbInterval_SelectedIndexChanged);
            // 
            // nudCategoryCount
            // 
            resources.ApplyResources(this.nudCategoryCount, "nudCategoryCount");
            this.nudCategoryCount.Name = "nudCategoryCount";
            this.nudCategoryCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudCategoryCount.ValueChanged += new System.EventHandler(this.nudCategoryCount_ValueChanged);
            // 
            // lblBreaks
            // 
            resources.ApplyResources(this.lblBreaks, "lblBreaks");
            this.lblBreaks.Name = "lblBreaks";
            // 
            // tabGraph
            // 
            this.tabGraph.Controls.Add(this.lblColumns);
            this.tabGraph.Controls.Add(this.nudColumns);
            this.tabGraph.Controls.Add(this.chkLog);
            this.tabGraph.Controls.Add(this.chkShowStd);
            this.tabGraph.Controls.Add(this.chkShowMean);
            this.tabGraph.Controls.Add(this.breakSliderGraph1);
            resources.ApplyResources(this.tabGraph, "tabGraph");
            this.tabGraph.Name = "tabGraph";
            this.tabGraph.UseVisualStyleBackColor = true;
            // 
            // lblColumns
            // 
            resources.ApplyResources(this.lblColumns, "lblColumns");
            this.lblColumns.Name = "lblColumns";
            // 
            // nudColumns
            // 
            this.nudColumns.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            resources.ApplyResources(this.nudColumns, "nudColumns");
            this.nudColumns.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudColumns.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudColumns.Name = "nudColumns";
            this.nudColumns.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudColumns.ValueChanged += new System.EventHandler(this.nudColumns_ValueChanged);
            // 
            // chkLog
            // 
            resources.ApplyResources(this.chkLog, "chkLog");
            this.chkLog.Name = "chkLog";
            this.chkLog.UseVisualStyleBackColor = true;
            this.chkLog.CheckedChanged += new System.EventHandler(this.chkLog_CheckedChanged);
            // 
            // chkShowStd
            // 
            resources.ApplyResources(this.chkShowStd, "chkShowStd");
            this.chkShowStd.Name = "chkShowStd";
            this.chkShowStd.UseVisualStyleBackColor = true;
            this.chkShowStd.CheckedChanged += new System.EventHandler(this.chkShowStd_CheckedChanged);
            // 
            // chkShowMean
            // 
            resources.ApplyResources(this.chkShowMean, "chkShowMean");
            this.chkShowMean.Name = "chkShowMean";
            this.chkShowMean.UseVisualStyleBackColor = true;
            this.chkShowMean.CheckedChanged += new System.EventHandler(this.chkShowMean_CheckedChanged);
            // 
            // breakSliderGraph1
            // 
            this.breakSliderGraph1.AttributeSource = null;
            this.breakSliderGraph1.BackColor = System.Drawing.Color.White;
            this.breakSliderGraph1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.breakSliderGraph1.BreakColor = System.Drawing.Color.Blue;
            this.breakSliderGraph1.BreakSelectedColor = System.Drawing.Color.Red;
            this.breakSliderGraph1.FontColor = System.Drawing.Color.Black;
            this.breakSliderGraph1.IntervalMethod = DotSpatial.Symbology.IntervalMethod.EqualInterval;
            resources.ApplyResources(this.breakSliderGraph1, "breakSliderGraph1");
            this.breakSliderGraph1.LogY = false;
            this.breakSliderGraph1.MaximumSampleSize = 10000;
            this.breakSliderGraph1.MinHeight = 20;
            this.breakSliderGraph1.Name = "breakSliderGraph1";
            this.breakSliderGraph1.NumColumns = 40;
            this.breakSliderGraph1.RasterLayer = null;
            this.breakSliderGraph1.Scheme = null;
            this.breakSliderGraph1.ShowMean = false;
            this.breakSliderGraph1.ShowStandardDeviation = false;
            this.breakSliderGraph1.Title = "Statistical Breaks:";
            this.breakSliderGraph1.TitleColor = System.Drawing.Color.Black;
            this.breakSliderGraph1.TitleFont = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            this.breakSliderGraph1.SliderMoved += new System.EventHandler<DotSpatial.Symbology.Forms.BreakSliderEventArgs>(this.breakSliderGraph1_SliderMoved);
            // 
            // dgvCategories
            // 
            this.dgvCategories.AllowUserToAddRows = false;
            this.dgvCategories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategories.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSymbol,
            this.colValues,
            this.colLegendText,
            this.colCount});
            resources.ApplyResources(this.dgvCategories, "dgvCategories");
            this.dgvCategories.MultiSelect = false;
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.RowHeadersVisible = false;
            this.dgvCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colSymbol
            // 
            this.colSymbol.FillWeight = 49.97129F;
            resources.ApplyResources(this.colSymbol, "colSymbol");
            this.colSymbol.Name = "colSymbol";
            this.colSymbol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colValues
            // 
            this.colValues.FillWeight = 142.132F;
            resources.ApplyResources(this.colValues, "colValues");
            this.colValues.Name = "colValues";
            this.colValues.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colLegendText
            // 
            this.colLegendText.FillWeight = 157.008F;
            resources.ApplyResources(this.colLegendText, "colLegendText");
            this.colLegendText.Name = "colLegendText";
            // 
            // colCount
            // 
            this.colCount.FillWeight = 50.88878F;
            resources.ApplyResources(this.colCount, "colCount");
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            this.colCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // chkHillshade
            // 
            resources.ApplyResources(this.chkHillshade, "chkHillshade");
            this.chkHillshade.Name = "chkHillshade";
            this.chkHillshade.UseVisualStyleBackColor = true;
            this.chkHillshade.CheckedChanged += new System.EventHandler(this.chkHillshade_CheckedChanged);
            // 
            // btnShadedRelief
            // 
            resources.ApplyResources(this.btnShadedRelief, "btnShadedRelief");
            this.btnShadedRelief.Name = "btnShadedRelief";
            this.btnShadedRelief.UseVisualStyleBackColor = true;
            this.btnShadedRelief.Click += new System.EventHandler(this.btnShadedRelief_Click);
            // 
            // grpHillshade
            // 
            this.grpHillshade.Controls.Add(this.btnShadedRelief);
            this.grpHillshade.Controls.Add(this.btnElevation);
            this.grpHillshade.Controls.Add(this.dbxElevationFactor);
            this.grpHillshade.Controls.Add(this.angLightDirection);
            this.grpHillshade.Controls.Add(this.chkHillshade);
            resources.ApplyResources(this.grpHillshade, "grpHillshade");
            this.grpHillshade.Name = "grpHillshade";
            this.grpHillshade.TabStop = false;
            // 
            // btnElevation
            // 
            this.btnElevation.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.ArrowDownGreen;
            resources.ApplyResources(this.btnElevation, "btnElevation");
            this.btnElevation.Name = "btnElevation";
            this.btnElevation.UseVisualStyleBackColor = true;
            this.btnElevation.Click += new System.EventHandler(this.btnElevation_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.FillWeight = 76.14214F;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.add;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.delete;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.opacityNoData);
            this.groupBox1.Controls.Add(this.colorNoData);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // opacityNoData
            // 
            this.opacityNoData.ColorButton = this.colorNoData;
            this.opacityNoData.FlipRamp = false;
            this.opacityNoData.FlipText = false;
            this.opacityNoData.InvertRamp = false;
            resources.ApplyResources(this.opacityNoData, "opacityNoData");
            this.opacityNoData.Maximum = 1D;
            this.opacityNoData.MaximumColor = System.Drawing.Color.Green;
            this.opacityNoData.Minimum = 0D;
            this.opacityNoData.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.opacityNoData.Name = "opacityNoData";
            this.opacityNoData.NumberFormat = "#.00";
            this.opacityNoData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.opacityNoData.RampRadius = 10F;
            this.opacityNoData.RampText = "Opacity";
            this.opacityNoData.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.opacityNoData.RampTextBehindRamp = true;
            this.opacityNoData.RampTextColor = System.Drawing.Color.Black;
            this.opacityNoData.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opacityNoData.ShowMaximum = false;
            this.opacityNoData.ShowMinimum = false;
            this.opacityNoData.ShowTicks = false;
            this.opacityNoData.ShowValue = false;
            this.opacityNoData.SliderColor = System.Drawing.Color.Blue;
            this.opacityNoData.SliderRadius = 4F;
            this.opacityNoData.TickColor = System.Drawing.Color.DarkGray;
            this.opacityNoData.TickSpacing = 5F;
            this.opacityNoData.Value = 0D;
            // 
            // colorNoData
            // 
            this.colorNoData.BevelRadius = 0;
            this.colorNoData.Color = System.Drawing.Color.Transparent;
            this.colorNoData.LaunchDialogOnClick = true;
            resources.ApplyResources(this.colorNoData, "colorNoData");
            this.colorNoData.Name = "colorNoData";
            this.colorNoData.RoundingRadius = 3;
            this.colorNoData.ColorChanged += new System.EventHandler(this.colorNoData_ColorChanged);
            // 
            // sldSchemeOpacity
            // 
            this.sldSchemeOpacity.ColorButton = null;
            this.sldSchemeOpacity.FlipRamp = false;
            this.sldSchemeOpacity.FlipText = false;
            resources.ApplyResources(this.sldSchemeOpacity, "sldSchemeOpacity");
            this.sldSchemeOpacity.InvertRamp = false;
            this.sldSchemeOpacity.Maximum = 1D;
            this.sldSchemeOpacity.MaximumColor = System.Drawing.Color.Green;
            this.sldSchemeOpacity.Minimum = 0D;
            this.sldSchemeOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldSchemeOpacity.Name = "sldSchemeOpacity";
            this.sldSchemeOpacity.NumberFormat = "#.00";
            this.sldSchemeOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldSchemeOpacity.RampRadius = 9F;
            this.sldSchemeOpacity.RampText = "Opacity";
            this.sldSchemeOpacity.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.sldSchemeOpacity.RampTextBehindRamp = true;
            this.sldSchemeOpacity.RampTextColor = System.Drawing.Color.Black;
            this.sldSchemeOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldSchemeOpacity.ShowMaximum = false;
            this.sldSchemeOpacity.ShowMinimum = false;
            this.sldSchemeOpacity.ShowTicks = false;
            this.sldSchemeOpacity.ShowValue = false;
            this.sldSchemeOpacity.SliderColor = System.Drawing.Color.Blue;
            this.sldSchemeOpacity.SliderRadius = 4F;
            this.sldSchemeOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.sldSchemeOpacity.TickSpacing = 5F;
            this.sldSchemeOpacity.Value = 1D;
            this.sldSchemeOpacity.ValueChanged += new System.EventHandler(this.sldSchemeOpacity_ValueChanged);
            // 
            // mwProgressBar1
            // 
            this.mwProgressBar1.FontColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.mwProgressBar1, "mwProgressBar1");
            this.mwProgressBar1.Name = "mwProgressBar1";
            this.mwProgressBar1.ShowMessage = true;
            // 
            // tccColorRange
            // 
            this.tccColorRange.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tccColorRange.HueShift = 0;
            resources.ApplyResources(this.tccColorRange, "tccColorRange");
            this.tccColorRange.Name = "tccColorRange";
            this.tccColorRange.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tccColorRange.UseRangeChecked = true;
            this.tccColorRange.ColorChanged += new System.EventHandler<DotSpatial.Symbology.Forms.ColorRangeEventArgs>(this.tccColorRange_ColorChanged);
            // 
            // RasterCategoryControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sldSchemeOpacity);
            this.Controls.Add(this.grpHillshade);
            this.Controls.Add(this.mwProgressBar1);
            this.Controls.Add(this.btnQuick);
            this.Controls.Add(this.tccColorRange);
            this.Controls.Add(this.tabScheme);
            this.Controls.Add(this.dgvCategories);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRamp);
            this.Controls.Add(this.cmdRefresh);
            this.Name = "RasterCategoryControl";
            resources.ApplyResources(this, "$this");
            this.tabScheme.ResumeLayout(false);
            this.tabStatistics.ResumeLayout(false);
            this.tabStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigFig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoryCount)).EndInit();
            this.tabGraph.ResumeLayout(false);
            this.tabGraph.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();
            this.grpHillshade.ResumeLayout(false);
            this.grpHillshade.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region ICategoryControl Members

        /// <summary>
        /// Initializes the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public void Initialize(ILayer layer)
        {
            Initialize(layer as IRasterLayer);
        }

        #endregion

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            _symbolizer.EditorSettings.RampColors = false;
            RefreshValues();
        }

        private void btnRamp_Click(object sender, EventArgs e)
        {
            _symbolizer.EditorSettings.RampColors = true;
            RefreshValues();
        }

        private void GetSettings()
        {
            _ignoreRefresh = true;
            EditorSettings settings = _symbolizer.EditorSettings;
            tccColorRange.Initialize(new ColorRangeEventArgs(settings.StartColor, settings.EndColor, settings.HueShift, settings.HueSatLight, settings.UseColorRange));
            UpdateTable();
            cmbInterval.SelectedItem = settings.IntervalMethod.ToString();
            UpdateStatistics(false);
            nudCategoryCount.Value = _newScheme.EditorSettings.NumBreaks;
            cmbIntervalSnapping.SelectedItem = settings.IntervalSnapMethod;
            nudSigFig.Value = settings.IntervalRoundingDigits;
            angLightDirection.Angle = (int)_symbolizer.ShadedRelief.LightDirection;
            dbxElevationFactor.Value = _symbolizer.ShadedRelief.ElevationFactor;
            chkHillshade.Checked = _symbolizer.ShadedRelief.IsUsed;
            colorNoData.Color = _symbolizer.NoDataColor;
            opacityNoData.Value = _symbolizer.NoDataColor.GetOpacity();
            _ignoreRefresh = false;
        }

        private void SetSettings()
        {
            if (_ignoreRefresh) return;
            EditorSettings settings = _symbolizer.EditorSettings;
            settings.NumBreaks = (int)nudCategoryCount.Value;
            settings.IntervalSnapMethod = (IntervalSnapMethod)cmbIntervalSnapping.SelectedItem;
            settings.IntervalRoundingDigits = (int)nudSigFig.Value;
        }

        private void UpdateStatistics(bool clear)
        {
            // Graph
            SetSettings();
            breakSliderGraph1.RasterLayer = _newLayer;
            breakSliderGraph1.Title = _newLayer.LegendText;
            breakSliderGraph1.ResetExtents();
            if (_symbolizer.EditorSettings.IntervalMethod == IntervalMethod.Manual && !clear)
            {
                breakSliderGraph1.UpdateBreaks();
            }
            else
            {
                breakSliderGraph1.ResetBreaks(null);
            }
            Statistics stats = breakSliderGraph1.Statistics;

            // Stat list
            dgvStatistics.Rows.Clear();
            dgvStatistics.Rows.Add(7);
            dgvStatistics[0, 0].Value = "Count";
            dgvStatistics[1, 0].Value = _raster.NumValueCells.ToString("#, ###");
            dgvStatistics[0, 1].Value = "Min";
            dgvStatistics[1, 1].Value = _raster.Minimum.ToString("#, ###");
            dgvStatistics[0, 2].Value = "Max";
            dgvStatistics[1, 2].Value = _raster.Maximum.ToString("#, ###");
            dgvStatistics[0, 3].Value = "Sum";
            dgvStatistics[1, 3].Value = (_raster.Mean * _raster.NumValueCells).ToString("#, ###");
            dgvStatistics[0, 4].Value = "Mean";
            dgvStatistics[1, 4].Value = _raster.Mean.ToString("#, ###");
            dgvStatistics[0, 5].Value = "Median";
            dgvStatistics[1, 5].Value = stats.Median.ToString("#, ###");
            dgvStatistics[0, 6].Value = "Std";
            dgvStatistics[1, 6].Value = stats.StandardDeviation.ToString("#, ###");
        }

        private void nudCategoryCount_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreRefresh) return;
            _ignoreRefresh = true;
            cmbInterval.SelectedItem = IntervalMethod.EqualInterval.ToString();
            _ignoreRefresh = false;
            RefreshValues();
        }

        private void chkLog_CheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.LogY = chkLog.Checked;
        }

        private void chkShowMean_CheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.ShowMean = chkShowMean.Checked;
        }

        private void chkShowStd_CheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.ShowStandardDeviation = chkShowStd.Checked;
        }

        private void cmbInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            IntervalMethod method = (IntervalMethod)Enum.Parse(typeof(IntervalMethod), cmbInterval.SelectedItem.ToString());
            if (_symbolizer == null) return;
            _symbolizer.EditorSettings.IntervalMethod = method;
            RefreshValues();
        }

        private void breakSliderGraph1_SliderMoved(object sender, BreakSliderEventArgs e)
        {
            _ignoreRefresh = true;
            cmbInterval.SelectedItem = "Manual";
            _ignoreRefresh = false;
            _symbolizer.EditorSettings.IntervalMethod = IntervalMethod.Manual;
            int index = _newScheme.Categories.IndexOf(e.Slider.Category as IColorCategory);
            if (index == -1) return;
            UpdateTable();
            dgvCategories.Rows[index].Selected = true;
        }

        private void nudColumns_ValueChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.NumColumns = (int)nudColumns.Value;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            nudCategoryCount.Value += 1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0) return;
            List<IColorCategory> deleteList = new List<IColorCategory>();
            ColorCategoryCollection categories = _newScheme.Categories;
            int count = 0;
            foreach (DataGridViewRow row in dgvCategories.SelectedRows)
            {
                int index = dgvCategories.Rows.IndexOf(row);
                deleteList.Add(categories[index]);
                count++;
            }
            foreach (IColorCategory category in deleteList)
            {
                int index = categories.IndexOf(category);
                if (index > 0 && index < categories.Count - 1)
                {
                    categories[index - 1].Maximum = categories[index + 1].Minimum;
                    categories[index - 1].ApplyMinMax(_newScheme.EditorSettings);
                }
                _newScheme.RemoveCategory(category);
                breakSliderGraph1.UpdateBreaks();
            }
            UpdateTable();
            _newScheme.EditorSettings.IntervalMethod = IntervalMethod.Manual;
            _newScheme.EditorSettings.NumBreaks -= count;
            UpdateStatistics(false);
        }

        private void nudSigFig_ValueChanged(object sender, EventArgs e)
        {
            if (_newScheme == null) return;
            _newScheme.EditorSettings.IntervalRoundingDigits = (int)nudSigFig.Value;

            RefreshValues();
        }

        private void cmbIntervalSnapping_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_newScheme == null) return;
            IntervalSnapMethod method = (IntervalSnapMethod)cmbIntervalSnapping.SelectedItem;
            _newScheme.EditorSettings.IntervalSnapMethod = method;
            switch (method)
            {
                case IntervalSnapMethod.SignificantFigures:
                    lblSigFig.Visible = true;
                    nudSigFig.Visible = true;
                    nudSigFig.Minimum = 1;
                    lblSigFig.Text = "Significant Figures:";
                    break;
                case IntervalSnapMethod.Rounding:
                    nudSigFig.Visible = true;
                    lblSigFig.Visible = true;
                    nudSigFig.Minimum = 0;
                    lblSigFig.Text = "Rounding Digits:";
                    break;
                case IntervalSnapMethod.None:
                    lblSigFig.Visible = false;
                    nudSigFig.Visible = false;
                    break;
                case IntervalSnapMethod.DataValue:
                    lblSigFig.Visible = false;
                    nudSigFig.Visible = false;
                    break;
            }

            RefreshValues();
        }

        /// <summary>
        /// Handles disposing unmanaged memory
        /// </summary>
        /// <param name="disposing">The disposed item</param>
        protected override void Dispose(bool disposing)
        {
            breakSliderGraph1.Dispose();
            if (components != null && disposing)
            {
                foreach (var control in components.Components)
                {
                    var id = control as IDisposable;
                    if (id != null)
                    {
                        id.Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }

        private void tccColorRange_ColorChanged(object sender, ColorRangeEventArgs e)
        {
            if (_ignoreRefresh) return;
            RasterEditorSettings settings = _newScheme.EditorSettings;
            settings.StartColor = e.StartColor;
            settings.EndColor = e.EndColor;
            settings.UseColorRange = e.UseColorRange;
            settings.HueShift = e.HueShift;
            settings.HueSatLight = e.HSL;
            RefreshValues();
        }

        private void btnQuick_Click(object sender, EventArgs e)
        {
            _quickSchemes.Show(btnQuick, new Point(0, 0));
        }

        private void QuickSchemeClicked(object sender, EventArgs e)
        {
            _ignoreRefresh = true;
            _newScheme.EditorSettings.NumBreaks = 2;
            nudCategoryCount.Value = 2;
            _ignoreRefresh = false;
            MenuItem mi = sender as MenuItem;
            if (mi == null) return;
            ColorSchemeType cs = (ColorSchemeType)Enum.Parse(typeof(ColorSchemeType), mi.Text);
            _newScheme.ApplyScheme(cs, _raster);
            UpdateTable();
            UpdateStatistics(true); // if the parameter is true, even on manual, the breaks are reset.
            breakSliderGraph1.Invalidate();
        }

        private void chkHillshade_CheckedChanged(object sender, EventArgs e)
        {
            _symbolizer.ShadedRelief.IsUsed = chkHillshade.Checked;
        }

        private void btnShadedRelief_Click(object sender, EventArgs e)
        {
            _shadedReliefDialog.PropertyGrid.SelectedObject = _symbolizer.ShadedRelief.Copy();
            _shadedReliefDialog.ShowDialog();
        }

        private void PropertyDialogChangesApplied(object sender, EventArgs e)
        {
            _symbolizer.ShadedRelief = (_shadedReliefDialog.PropertyGrid.SelectedObject as IShadedRelief).Copy();
            angLightDirection.Angle = (int)_symbolizer.ShadedRelief.LightDirection;
            dbxElevationFactor.Value = _symbolizer.ShadedRelief.ElevationFactor;
        }

        private void angLightDirection_AngleChanged(object sender, EventArgs e)
        {
            _symbolizer.ShadedRelief.LightDirection = angLightDirection.Angle;
        }

        private void dbxElevationFactor_TextChanged(object sender, EventArgs e)
        {
            _symbolizer.ShadedRelief.ElevationFactor = (float)dbxElevationFactor.Value;
        }

        private void dbxMin_TextChanged(object sender, EventArgs e)
        {
            _symbolizer.EditorSettings.Min = dbxMin.Value;
            _symbolizer.Scheme.CreateCategories(_raster);
            UpdateStatistics(true); // if the parameter is true, even on manual, the breaks are reset.
            UpdateTable();
        }

        private void dbxMax_TextChanged(object sender, EventArgs e)
        {
            _symbolizer.EditorSettings.Max = dbxMax.Value;
            _symbolizer.Scheme.CreateCategories(_raster);
            UpdateStatistics(true); // if the parameter is true, even on manual, the breaks are reset.
            UpdateTable();
        }

        private void btnElevation_Click(object sender, EventArgs e)
        {
            _elevationQuickPick.Show(grpHillshade, new Point(dbxElevationFactor.Left, btnElevation.Bottom));
        }

        private void sldSchemeOpacity_ValueChanged(object sender, EventArgs e)
        {
            _symbolizer.Opacity = Convert.ToSingle(sldSchemeOpacity.Value);
            foreach (var cat in _symbolizer.Scheme.Categories)
            {
                cat.HighColor = cat.HighColor.ToTransparent(_symbolizer.Opacity);
                cat.LowColor = cat.LowColor.ToTransparent(_symbolizer.Opacity);
            }
            dgvCategories.Invalidate();
        }

        private void colorNoData_ColorChanged(object sender, EventArgs e)
        {
            _symbolizer.NoDataColor = colorNoData.Color;
        }
    }
}