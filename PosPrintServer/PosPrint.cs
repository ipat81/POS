using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using Microsoft.PointOfService;
using System.IO;
using System.Reflection;
using System.Globalization;

using BOPr;

namespace PosPrintServer
{

    public class Printer
    {
                // A maximum of 2 line widths will be considered
        const int MAX_LINE_WIDTHS = 2;
        private Boolean HavePrinter;
		/// <summary>
		/// PosPrinter object.
		/// </summary>
		PosPrinter m_Printer = null;

    ///<summary>
	/// Description of FrameStep7.
	/// </summary>

		/// <summary>
		/// Main entry point.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            //Application.Run(new FrameStep7());
		}

#region singleton constructor
   private static volatile Printer instance;
   private static object syncRoot = new Object();

   private Printer() {}

   public static Printer Instance
   {
      get 
      {
         if (instance == null) 
         {
            lock (syncRoot) 
            {
               if (instance == null) 
                  instance = new Printer();
            }
         }

         return instance;
      }
   }

#endregion

            public int POSPrint(PosDoc printDoc)
            {
                int success=0;

                if (printDoc != null)
                {
                    if (!HavePrinter || m_Printer == null) GrabPrinter();
                    for (int i = 1; i <= printDoc.Copies; i++)

                    {
                        try
                        {
                            PrintHeader(printDoc);
                            PrintDetail(printDoc);
                            PrintFooter(printDoc);
                        }
                        catch (PosControlException pce)
                        {
                            if (pce.ErrorCode == ErrorCode.Failure && pce.ErrorCodeExtended == 0)
                            {
                                if (m_Printer != null) ReleasePrinter();
                            }
                            success = -1;
                        }

                    }
                    //ReleasePrinter();
                }
                return success;

            }
            void PrintHeader(PosDoc printDoc)
            {
                int i=0;
                ICollection strlines=printDoc.HeaderLines as Microsoft.VisualBasic.Collection ;
                m_Printer.PrintNormal(PrinterStation.Receipt,"\u001b|200uF");          
                foreach ( string strLine in printDoc.HeaderLines)
                {
                    if (i==0)
                    {
                        switch (printDoc.POSDocType)
                        {
                            case PosDoc.enumPOSDocType.GuestCheck:
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|4C" + strLine + "\n");
                                    break;
                            case PosDoc.enumPOSDocType.KOT:
                                    m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|2C" + strLine + "\n");
                                    break;
                        }
                }
                    else
                        m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|1C" + strLine + "\n");
                    i++;
                }
            }
        void PrintDetail(PosDoc printDoc)
        {
            foreach (string strLine in printDoc.DetailLines)
            {
                switch (printDoc.POSDocType)
                {
                    case PosDoc.enumPOSDocType.GuestCheck:
                        m_Printer.PrintNormal(PrinterStation.Receipt, strLine + "\n");
                            break;
                    case PosDoc.enumPOSDocType.KOT:
                            m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|2C" + strLine + "\n");
                            //m_Printer.PrintNormal(PrinterStation.Receipt, strLine + "\n");
                            //m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|1C" + "  " + "\n");
                            break;
                }

            }
        }

        void PrintFooter (PosDoc printDoc)
        {
            int i=0;
            foreach (string strLine in printDoc.FooterLines)
            {
                switch (printDoc.POSDocType)
                {
                    case PosDoc.enumPOSDocType.KOT:
                        if (i == 0)
                            {m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|4C" + strLine + "\n");}
                        else
                            {m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|bC" + strLine + "\n");}
                        break;
                    case PosDoc.enumPOSDocType.GuestCheck:
                        if (i == printDoc.FooterLines.Count - 1)
                            {m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|3C" + strLine + "\n");}
                        else
                            {m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b" + "|2C" + strLine + "\n");}
                            break;
                }
                i++;
            }
                m_Printer.PrintNormal(PrinterStation.Receipt,"\u001b|21F");    
            
                m_Printer.PrintNormal(PrinterStation.Receipt,"\u001b|75fP");          
        }
		/// The processing code required in order to enable to use of service is written here.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GrabPrinter()
		{
			//<<<step1>>>--Start
			//Use a Logical Device Name which has been set on the SetupPOS.
            //this name is for usb connection
            //string strLogicalName = "PosPrinterTM";
            //this name is for ethernet connection
            string strLogicalName = "PosPrinterTMIP";
						
			//Current Directory Path
            //string strCurDir = Directory.GetCurrentDirectory();

            //string strFilePath = strCurDir.Substring(0,strCurDir.LastIndexOf("Step7") + "Step7\\".Length);

            //strFilePath += "Logo.bmp";

            //if (!HavePrinter) SetPrinter(strLogicalName);
            SetPrinter(strLogicalName);
            
			//<<<step1>>>--End
            //Register OutputCompleteEventHandler.
            //AddOutputComplete(m_Printer);

            //Open the device
            //m_Printer.Open();

            //Get the exclusive control right for the opened device.
            //Then the device is disable from other application.
            //if (!m_Printer.Claimed) m_Printer.Claim(1000);

            //Enable the device.
            //m_Printer.DeviceEnabled = true;

            //<<<step3>>>--Start
            //Output by the high quality mode
            //m_Printer.RecLetterQuality = true; 

            //if (m_Printer.CapRecBitmap == true)
            //{

            //    bool bSetBitmapSuccess = false;
            //    for (int iRetryCount = 0; iRetryCount < 5; iRetryCount++)
            //    {
            //        try
            //        {
            //            //<<<step5>>>--Start
            //            //Register a bitmap
            //            m_Printer.SetBitmap(1, PrinterStation.Receipt,
            //                strFilePath, m_Printer.RecLineWidth / 2,
            //                PosPrinter.PrinterBitmapCenter);
            //            //<<<step5>>>--End
            //            bSetBitmapSuccess = true;
            //            break;
            //        }
            //        catch (PosControlException pce)
            //        {
            //            if (pce.ErrorCode == ErrorCode.Failure && pce.ErrorCodeExtended == 0 && pce.Message == "It is not initialized.")
            //            {
            //                System.Threading.Thread.Sleep(1000);
            //            }
            //        }
            //    }
            //    if (!bSetBitmapSuccess)
            //    {
            //        //MessageBox.Show("Failed to set bitmap.", "Printer_SampleStep7"
            //        //        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            //<<<step3>>>--End

            //<<<step5>>>--Start
            // Even if using any printers, 0.01mm unit makes it possible to print neatly.
            //m_Printer.MapMode = MapMode.Metric;
            //m_Printer.RecEmpty
                //m_Printer.
            //<<<step5>>>--End

		}
        private void ClaimPrinter()
        {
            //Open the device
            m_Printer.Open();

            //Get the exclusive control right for the opened device.
            //Then the device is disable from other application.
            if (!m_Printer.Claimed) m_Printer.Claim(1000);

            //Enable the device.
            m_Printer.DeviceEnabled = true;

            //<<<step3>>>--Start
            //Output by the high quality mode
            m_Printer.RecLetterQuality = true;
            m_Printer.MapMode = MapMode.Metric;
        }
        private void SetPrinter(string strLogicalName)
        {
            try
            {
                //Create PosExplorer
                PosExplorer posExplorer = new PosExplorer();

                DeviceInfo deviceInfo = null;

                try
                {
                    //if (m_Printer != null || m_Printer.State == ControlState.Error)
                    if (m_Printer != null )
                            ReleasePrinter();
                    deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter, strLogicalName);
                    m_Printer = (PosPrinter)posExplorer.CreateInstance(deviceInfo);
                    if (m_Printer != null)
                    {
                        ClaimPrinter();
                        HavePrinter = true;
                    }
                    //if (m_Printer.Claimed) m_Printer.Release();
                    
                }
                catch (Exception)
                {
                    HavePrinter = false;
                    m_Printer = null;
                    //ChangeButtonStatus();
                    //return;
                }

            }
            catch (PosControlException)
            {
                HavePrinter = false;
                //ChangeButtonStatus();
            }
        }

		/// <summary>
		/// When the method "closing" is called,
		/// the following code is run.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ReleasePrinter()
		{
			//<<<step1>>>--Start
			if(m_Printer != null && HavePrinter)
			{
				try
				{
                    
					//Cancel the device
                    //m_Printer.DeviceEnabled = false;
                    m_Printer.Close();
					//Release the device exclusive control right.
                    m_Printer.Release();
                    m_Printer = null;
                    HavePrinter = false; 

				}
				catch(PosException)
				{
				}
				finally
				{
					//Finish using the device.
                    //m_Printer.Close();
                    HavePrinter = false;
                }
			}
			//<<<step1>>>--End
		}

		/// <summary>
		/// Add OutputCompeleteEventHandler.
		/// </summary>
		/// <param name="eventSource"></param>
        //protected void AddOutputComplete(object eventSource)
        //{
        //    //<<<step7>>>--Start
        //    EventInfo outputCompleteEvent = eventSource.GetType().GetEvent("OutputCompleteEvent");
        //    if (outputCompleteEvent != null)
        //    {
        //        outputCompleteEvent.AddEventHandler(eventSource,
        //            new OutputCompleteEventHandler(OnOutputCompleteEvent));
        //    }
        //    //<<<step7>>>--End
        //}

		/// <summary>
		/// Remove OutputCompeleteEventHandler.
		/// </summary>
		/// <param name="eventSource"></param>
        //protected void RemoveOutputComplete(object eventSource)
        //{
        //    //<<<step7>>>--Start
        //    EventInfo outputCompleteEvent = eventSource.GetType().GetEvent( "OutputCompleteEvent");
        //    if( outputCompleteEvent != null )
        //    {
        //        outputCompleteEvent.RemoveEventHandler( eventSource,
        //            new OutputCompleteEventHandler(OnOutputCompleteEvent));
        //    }
        //    //<<<step7>>>--End
        //}

		/// <summary>
		/// OutputComplete Event
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
        //protected  void OnOutputCompleteEvent(object source, OutputCompleteEventArgs e)
        ////{
        ////    //<<<step7>>>--Start
        ////    //Notify that printing is completed when it is asnchronous.
        ////    MessageBox.Show("Complete printing : ID = " + e.OutputId.ToString()
        ////        ,"PrinterSample_Step7");
        ////    //<<<step7>>>--End
        ////}

	}
}
