﻿//
// Copyright (c) Antmicro
//
// This file is part of the Emul8 project.
// Full license details are defined in the 'LICENSE' file.
//
using System;

namespace Emul8.Peripherals.Wireless
{
    public static class SimpleMediumExtension
    {
        public static void SetSimpleWirelessFunction(this WirelessMedium wirelessMedium)
        {
            wirelessMedium.SetMediumFunction(SimpleMediumFunction.Instance);
        }
    }

    public sealed class SimpleMediumFunction : IMediumFunction
    {
        public static SimpleMediumFunction Instance { get; private set; }

        static SimpleMediumFunction()
        {
            Instance = new SimpleMediumFunction();
        }

        private SimpleMediumFunction()
        {
        }

        public bool CanReach(Position from, Position to)
        {
            // always can reach destination - that's why we call it `simple`
            return true;
        }

        public bool CanTransmit(Position from)
        {
            // always can transmit - that's why we call it `simple`
            return true;
        }

        public string FunctionName { get { return Name; } }

        private const string Name = "simple_medium_function";
    }
}
