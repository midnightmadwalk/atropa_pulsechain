﻿#pragma warning disable CS8981

using Dysnomia.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dysnomia.Domain.bin
{
    internal class connect : Command
    {
        new public static String Name = "cmd_Connect";
        new public static String Description = "Initiate A New Connection connect [Host] [Port]";

        protected override void Phi()
        {
            if (Theta == null) throw new Exception("Null Command Theta");
            if (Args == null) throw new Exception("Null Command Args");
            byte[] From = Encoding.Default.GetBytes(Name);

            try
            {
                if (Args.Length == 0)
                    Theta.Out.Enqueue(new Tare.MSG(From, Encoding.Default.GetBytes("Connect Command Requires At Least 1 Argument"), 6));
                else
                {
                    Theta.In.Enqueue(new Tare.MSG(From, new byte[] { 0x05 }, 6));
                    Theta.In.Enqueue(new Tare.MSG(From, Encoding.Default.GetBytes(Args[0]), 6));
                    if (Args.Length < 2) Theta.In.Enqueue(new Tare.MSG(From, BitConverter.GetBytes(Int16.Parse("5555")), 6)); // 0x15B3 = 5555
                    else Theta.In.Enqueue(new Tare.MSG(From, BitConverter.GetBytes(Int16.Parse(Args[1])), 6));
                }
            } catch(Exception e)
            {
                Logging.Log(new Tare.MSG(Encoding.Default.GetBytes(Name), Encoding.Default.GetBytes(e.Message), 7));
                if(e.StackTrace != null) Logging.Log(new Tare.MSG(Encoding.Default.GetBytes(Name), Encoding.Default.GetBytes(e.StackTrace), 7));
                Theta.In.Clear();
                Theta.Out.Enqueue(new Tare.MSG(From, Encoding.Default.GetBytes("Unable To Connect: " + Args[0] + " Port " + Args[1]), 6));
            }
        }
    }
}
