using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace NServiceBus.Modeling.EndpointDesign
{
    internal partial class EndpointDesignCommandSet
    {
        private Guid guidSaveAsImageMenuCmdSet = new Guid("8337AB14-7F28-4183-A5E0-42A89C0ED16F");
        private const int grpMenu = 0x01001;
        private const int cmdSaveAsImageMenuCommand = 1;

        protected override IList<MenuCommand> GetMenuCommands()
        {
            // Get the list of generated commands.
            IList<MenuCommand> commands = base.GetMenuCommands();
            
            // Add a custom command:
            DynamicStatusMenuCommand SaveAsImageCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusSaveAsImageCommand),
                new EventHandler(OnMenuSaveAsImageCommand),
                new CommandID(guidSaveAsImageMenuCmdSet, cmdSaveAsImageMenuCommand));

            commands.Add(SaveAsImageCommand);

            // Add more commands here.
            return commands;
        }

        private void OnStatusSaveAsImageCommand(object sender, EventArgs e)
        {
            MenuCommand command = sender as MenuCommand;
            command.Enabled = true;
        }

        private void OnMenuSaveAsImageCommand(object sender, EventArgs e)
        {
            // Find diagram
            var rootElement = this.CurrentDocData.RootElement;
            var diagrams = PresentationViewsSubject.GetPresentation(rootElement);
            var diagram = diagrams.FirstOrDefault() as EndpointDesignDiagram;

            // Save diagram as Image file
            var filename = Path.ChangeExtension(this.CurrentDocData.FileName, "png");

            Bitmap bitmap = diagram.CreateBitmap(new [] { diagram }, Diagram.CreateBitmapPreference.FavorClarityOverSmallSize);
            
            this.CropImage(bitmap).Save(filename, ImageFormat.Png);
            System.Diagnostics.Process.Start(filename);
        }

        private Bitmap CropImage(Bitmap originalBitmap)
        {
            // Find the min/max non-white/transparent pixels
            Point min = new Point(int.MaxValue, int.MaxValue);
            Point max = new Point(int.MinValue, int.MinValue);

            for (int x = 0; x < originalBitmap.Width; ++x)
            {
                for (int y = 0; y < originalBitmap.Height; ++y)
                {
                    Color pixelColor = originalBitmap.GetPixel(x, y);
                    if (!(pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                      || pixelColor.A < 255)
                    {
                        if (x < min.X) min.X = x;
                        if (y < min.Y) min.Y = y;

                        if (x > max.X) max.X = x;
                        if (y > max.Y) max.Y = y;
                    }
                }
            }

            // Adding margin
            const int margin = 25;
            min.X -= margin;
            min.Y -= margin;
            max.X += margin;
            max.Y += margin;

            if (min.X < 0) min.X = 0;
            if (min.Y < 0) min.Y = 0;

            // Create a new bitmap from the crop rectangle
            Rectangle cropRectangle = new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
            Bitmap newBitmap = new Bitmap(cropRectangle.Width, cropRectangle.Height);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.DrawImage(originalBitmap, 0, 0, cropRectangle, GraphicsUnit.Pixel);
            }

            return newBitmap;
        }
    }
}
