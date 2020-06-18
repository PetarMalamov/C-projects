using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Draw.src.Model;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		private PointF mouseLocation;
		private Shape copyShape;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			ColorPicker.Color = Color.Peru;
			dialogProcessor.FillColor = Color.Peru;
			dialogProcessor.Opacity = 255;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked)
			{
				if (e.Button==MouseButtons.Left)
				{
					var shp = dialogProcessor.ContainsPoint(e.Location);
					if (shp != null)
					{
						if (dialogProcessor.Selection.Contains(shp))
						{
							dialogProcessor.Selection.Remove(shp);
							shp.BorderColor = Color.Black;
						}
						else
						{
							dialogProcessor.Selection.Add(shp);
							shp.BorderColor = Color.Red;
						}

						statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
						dialogProcessor.IsDragging = true;
						dialogProcessor.LastLocation = e.Location;
						viewPort.Invalidate();
					}
					
				}
				else if (e.Button==MouseButtons.Right)
				{
					Shape shape = dialogProcessor.ContainsPoint(e.Location);
					copyShape = shape;
					if (shape!=null)
					{
						MouseMenu.Show(e.Location);
					}
					else
					{
						mouseLocation = e.Location;
						PanelMouseMenu.Show(e.Location);
					}
				}
			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

		private void TriangleAddButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomTriangle();

			statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник.";

			viewPort.Invalidate();
		}

		private void ElipseAddButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomElipse();

			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса.";

			viewPort.Invalidate();
		}

		private void EighthAddButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomEighth();

			statusBar.Items[0].Text = "Последно действие: Рисуване на осмица.";

			viewPort.Invalidate();
		}



		private void LineWidth_Click(object sender, EventArgs e)
		{
			statusBar.Items[0].Text = "Последно действие: Промяна на дебелината на линията.";
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			statusBar.Items[0].Text = "Последно действие: Промяна на дебелината на линията." + sender.ToString();
		}

		private void SelectedLineSize(object sender, EventArgs e)
		{
			int size = int.Parse(sender.ToString());
			dialogProcessor.SetLineWidth(size);
			statusBar.Items[0].Text = "Последно действие: Промяна на дебелината на линията." + sender.ToString();
			viewPort.Invalidate();
		}

		private void ShapeColor_Click(object sender, EventArgs e)
		{
			if (ColorPicker.ShowDialog()==DialogResult.OK)
			{
				dialogProcessor.FillColor = ColorPicker.Color;
				dialogProcessor.SetColor(dialogProcessor.FillColor);
				viewPort.Invalidate();
			}
		}

		private void GetOpacity(object sender, EventArgs e)
		{
			dialogProcessor.SetOpacity(sender.ToString(),copyShape);
			statusBar.Items[0].Text = "Последно действие: Промяна на прозрачност." + sender.ToString();
			viewPort.Invalidate();
		}

		private void Angle_Scroll(object sender, EventArgs e)
		{
			dialogProcessor.RotateShape(Angle.Value,null);
			statusBar.Items[0].Text = "Последно действие:завъртане.";
			viewPort.Invalidate();
		}

		private void WidthBar_Scroll(object sender, EventArgs e)
		{
			dialogProcessor.ChangeWidth(WidthBar.Value);
			statusBar.Items[0].Text = "Последно действие:уголемяване.";
			viewPort.Invalidate();
		}

		private void heigthBar_Scroll(object sender, EventArgs e)
		{
			dialogProcessor.ChangeHeigth(heigthBar.Value);
			statusBar.Items[0].Text = "Последно действие:уголемяване.";
			viewPort.Invalidate();
		}

		private void groupButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.GroupSelectedShapes();
			statusBar.Items[0].Text = "Последно действие:групиране.";
			viewPort.Invalidate();
		}

		private void ungroupButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.UnGroupSelectedShapes();
			statusBar.Items[0].Text = "Последно действие:групиране.";
			viewPort.Invalidate();
		}

		private void SetNameButton_Click(object sender, EventArgs e)
		{
			if (!ShapeName.Text.Equals("Set name"))
			{
				dialogProcessor.SetName(ShapeName.Text,null);
				statusBar.Items[0].Text = "Последно действие:задаване на име.";
				MessageBox.Show("Name is set");
				viewPort.Invalidate();
			}
		}

		private void byNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			String name = Interaction.InputBox("Enter name", "Search by name", "");
			dialogProcessor.SearchByName(name);
			statusBar.Items[0].Text = "Последно действие:търсене по име.";
			viewPort.Invalidate();
		}

		private void byColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			String color = Interaction.InputBox("Enter color", "Search by color", "");
			dialogProcessor.SearchByColor(color);
			statusBar.Items[0].Text = "Последно действие:търсене по цвят.";
			viewPort.Invalidate();
		}

		private void byTypeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.SearchByType(sender.ToString());
			statusBar.Items[0].Text = "Последно действие:търсене по тип.";
			viewPort.Invalidate();
		}

		private void RotateMouseMenu_Click(object sender, EventArgs e)
		{
			dialogProcessor.RotateShape(Int32.Parse(sender.ToString()),copyShape);
			statusBar.Items[0].Text = "Последно действие:завъртане.";
			viewPort.Invalidate();
		}


		private void setNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			String name = Interaction.InputBox("Enter name", "Search by name", "");
			dialogProcessor.SetName(name,copyShape);
			statusBar.Items[0].Text = "Последно действие:задаване на име.";
			viewPort.Invalidate();
		}

		private void setColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			String color = Interaction.InputBox("Enter color", "Search by color", "");
			dialogProcessor.SetColor(color,copyShape);
			statusBar.Items[0].Text = "Последно действие:задаване на цвят.";
			viewPort.Invalidate();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			dialogProcessor.DeleteSelected(copyShape);
			statusBar.Items[0].Text = "Последно действие:изтриване.";
			viewPort.Invalidate();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.CopyItems();
			statusBar.Items[0].Text = "Последно действие:копиране.";
			viewPort.Invalidate();
		}

		private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			dialogProcessor.PasteItems(mouseLocation);
			statusBar.Items[0].Text = "Последно действие:копиране.";
			viewPort.Invalidate();
		}

		private void clearPanelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.ClearAll();
			statusBar.Items[0].Text = "Последно действие:копиране.";
			viewPort.Invalidate();
		}

		private void copyToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			dialogProcessor.CopyItems(copyShape);
			statusBar.Items[0].Text = "Последно действие:копиране.";
			viewPort.Invalidate();
		}

		private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog savePicture = new SaveFileDialog();
			savePicture.CheckFileExists = false;
			savePicture.CheckPathExists = true;
			savePicture.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
			savePicture.InitialDirectory = @"C:\Users\";

			DialogResult result = savePicture.ShowDialog();
			if (result==DialogResult.OK)
			{
				using (var bmp = new Bitmap(viewPort.Width, viewPort.Height))
				{
					viewPort.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
					bmp.Save(savePicture.FileName);
				}
			}
		}

		private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog openPicture = new SaveFileDialog();
			openPicture.CheckFileExists = false;
			openPicture.CheckPathExists = true;
			openPicture.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

			DialogResult result = openPicture.ShowDialog();
			if (result == DialogResult.OK)
			{
				viewPort.BackgroundImage = Image.FromFile(openPicture.FileName);
			}
			viewPort.Invalidate();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog savePicture = new SaveFileDialog();
			savePicture.Filter = "Bin(*.bin) | *.bin";
			savePicture.InitialDirectory = @"C:\Users\";

			DialogResult result = savePicture.ShowDialog();
			if (result==DialogResult.OK)
			{
				using (Stream stream = File.Open(savePicture.FileName, FileMode.Create))
				{
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(stream, dialogProcessor.ShapeList);
				}
			}

		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog openPicture = new SaveFileDialog();
			openPicture.Filter = "Bin(*.bin) | *.bin";

			DialogResult result = openPicture.ShowDialog();
			if (result == DialogResult.OK)
			{
				using (Stream stream = File.Open(openPicture.FileName, FileMode.Open))
				{
					BinaryFormatter bin = new BinaryFormatter();
					var shapes = (List<Shape>)bin.Deserialize(stream);
					dialogProcessor.ShapeList = shapes;
				}
			}
			viewPort.Invalidate();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomLetter();

			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

			viewPort.Invalidate();
		}
	}
}
