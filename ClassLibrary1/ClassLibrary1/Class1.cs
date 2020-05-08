using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

namespace ClassLibrary1
{
    public class Class1
    {
        [CommandMethod("CreateUcs")]
        public void Test()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Editor ed = document.Editor;
            Matrix3d mt= ed.CurrentUserCoordinateSystem;
            Point3d basePt = new Point3d(1000, 1000, 0);
            Vector3d vecX = new Vector3d(100, 100, 0);
            Vector3d vecY = vecX.GetPerpendicularVector();
            using (Transaction trans=document.TransactionManager.StartTransaction())
            {
                UcsTable ut = trans.GetObject(document.Database.UcsTableId, OpenMode.ForRead) as UcsTable;
                UcsTableRecord utr = new UcsTableRecord();
                utr.Name = "TestUcs";
                utr.Origin = basePt;
                utr.XAxis = vecX;
                utr.XAxis = vecY;
                ut.UpgradeOpen();
                ut.Add(utr);
                ut.DowngradeOpen();
                trans.AddNewlyCreatedDBObject(utr, true);
                trans.Commit();
            }
        }
        [CommandMethod("TransText")]
        public void Test()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            using (Transaction trans = document.TransactionManager.StartTransaction())
            {
                UcsTable ut = trans.GetObject(document.Database.UcsTableId, OpenMode.ForRead) as UcsTable;
                UcsTableRecord utr = trans.GetObject(ut["TestUcs"],OpenMode.ForRead); 
                utr.Name = "TestUcs";
                utr.Origin = basePt;
                utr.XAxis = vecX;
                utr.XAxis = vecY;
                ut.Add(utr);
                trans.AddNewlyCreatedDBObject(utr, true);
                trans.Commit();
            }
        }
    }
}
