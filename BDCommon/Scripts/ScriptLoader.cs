/*
 * This file is part of black desert-emu <http://necroz-project.net/>.
 *  
 * black desert-emu is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * black desert-emu is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with black desert-emu. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.CodeDom.Compiler;

using NLog;

namespace BDCommon.Scripts
{
    /* *
    * @author Maxes727
    */
    public class ScriptLoader<T>
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected ICodeCompiler Icc = new Microsoft.CSharp.CSharpCodeProvider().CreateCompiler();

        public Type LoadScript(string filePath)
        {
            CompilerResults cr = Icc.CompileAssemblyFromFile(
                new CompilerParameters(
                    new[]
                    {
                        "System.Data.dll",
                        "System.XML.dll",
                        "System.Linq.dll",
                        "System.Drawing.dll",
                        "System.dll",
                        "BDCommon.dll",
                        "NLog.dll",
                        "BlackDesertGame.exe"
                    },
                    null,
                    true), filePath);

            if (cr.Errors != null && cr.Errors.Count > 0)
            {
                foreach (CompilerError error in cr.Errors)
                {
                    Log.Error("Error load command {0} in line {2}"+ Environment.NewLine +"{1}", error.FileName, error.ErrorText, error.Line);
                }
                return null;
            }

            Type[] types = cr.CompiledAssembly.GetTypes();
            Type[] interfaces = types[0].GetInterfaces();
            if (interfaces.Length < 1)
            {
                Log.Error("Script type not found in {0}", filePath);
                return null;
            }
            if (interfaces[0] == typeof (T))
                return types[0];
            return null;

        }
    }
}
