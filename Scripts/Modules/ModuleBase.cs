﻿// Copyright (C) 2003, 2004 Jason Bevins
//
// This library is free software; you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2.1 of the License, or (at
// your option) any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
// License (COPYING.txt) for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation,
// Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// The developer's email is jlbezigvins@gmzigail.com (for great email, take
// off every 'zig'.)
//
using UnityEngine;

namespace M8.Noise.Module {
    public struct Global {
        public static int randomSeed { get { return mSeed; } set { mSeed = value; } }

        private static int mSeed;
    }

    /// Abstract base class for noise modules.
    ///
    /// A <i>noise module</i> is an object that calculates and outputs a value
    /// given a three-dimensional input value.
    ///
    /// Each type of noise module uses a specific method to calculate an
    /// output value.  Some of these methods include:
    ///
    /// - Calculating a value using a coherent-noise function or some other
    ///   mathematical function.
    /// - Mathematically changing the output value from another noise module
    ///   in various ways.
    /// - Combining the output values from two noise modules in various ways.
    ///
    /// An application can use the output values from these noise modules in
    /// the following ways:
    ///
    /// - It can be used as an elevation value for a terrain height map
    /// - It can be used as a grayscale (or an RGB-channel) value for a
    ///   procedural texture
    /// - It can be used as a position value for controlling the movement of a
    ///   simulated lifeform.
    ///
    /// A noise module defines a near-infinite 3-dimensional texture.  Each
    /// position in this "texture" has a specific value.
    ///
    /// <b>Combining noise modules</b>
    ///
    /// Noise modules can be combined with other noise modules to generate
    /// complex output values.  A noise module that is used as a source of
    /// output values for another noise module is called a <i>source
    /// module</i>.  Each of these source modules may be connected to other
    /// source modules, and so on.
    ///
    /// There is no limit to the number of noise modules that can be connected
    /// together in this way.  However, each connected noise module increases
    /// the time required to calculate an output value.
    ///
    /// <b>Noise-module categories</b>
    ///
    /// The noise module classes that are included in libnoise can be roughly
    /// divided into five categories.
    ///
    /// <i>Generator Modules</i>
    ///
    /// A generator module outputs a value generated by a coherent-noise
    /// function or some other mathematical function.
    ///
    /// Examples of generator modules include:
    /// - noise::module::Const: Outputs a constant value.
    /// - noise::module::Perlin: Outputs a value generated by a Perlin-noise
    ///   function.
    /// - noise::module::Voronoi: Outputs a value generated by a Voronoi-cell
    ///   function.
    ///
    /// <i>Modifier Modules</i>
    ///
    /// A modifer module mathematically modifies the output value from a
    /// source module.
    ///
    /// Examples of modifier modules include:
    /// - noise::module::Curve: Maps the output value from the source module
    ///   onto an arbitrary function curve.
    /// - noise::module::Invert: Inverts the output value from the source
    ///   module.
    ///
    /// <i>Combiner Modules</i>
    ///
    /// A combiner module mathematically combines the output values from two
    /// or more source modules together.
    ///
    /// Examples of combiner modules include:
    /// - noise::module::Add: Adds the two output values from two source
    ///   modules.
    /// - noise::module::Max: Outputs the larger of the two output values from
    ///   two source modules.
    ///
    /// <i>Selector Modules</i>
    ///
    /// A selector module uses the output value from a <i>control module</i>
    /// to specify how to combine the output values from its source modules.
    ///
    /// Examples of selector modules include:
    /// - noise::module::Blend: Outputs a value that is linearly interpolated
    ///   between the output values from two source modules; the interpolation
    ///   weight is determined by the output value from the control module.
    /// - noise::module::Select: Outputs the value selected from one of two
    ///   source modules chosen by the output value from a control module.
    ///
    /// <i>Transformer Modules</i>
    ///
    /// A transformer module applies a transformation to the coordinates of
    /// the input value before retrieving the output value from the source
    /// module.  A transformer module does not modify the output value.
    ///
    /// Examples of transformer modules include:
    /// - RotatePoint: Rotates the coordinates of the input value around the
    ///   origin before retrieving the output value from the source module.
    /// - ScalePoint: Multiplies each coordinate of the input value by a
    ///   constant value before retrieving the output value from the source
    ///   module.
    ///
    /// <b>Connecting source modules to a noise module</b>
    ///
    /// An application connects a source module to a noise module by passing
    /// the source module to the SetSourceModule() method.
    ///
    /// The application must also pass an <i>index value</i> to
    /// SetSourceModule() as well.  An index value is a numeric identifier for
    /// that source module.  Index values are consecutively numbered starting
    /// at zero.
    ///
    /// To retrieve a reference to a source module, pass its index value to
    /// the GetSourceModule() method.
    ///
    /// Each noise module requires the attachment of a certain number of
    /// source modules before it can output a value.  For example, the
    /// noise::module::Add module requires two source modules, while the
    /// noise::module::Perlin module requires none.  Call the
    /// GetSourceModuleCount() method to retrieve the number of source modules
    /// required by that module.
    ///
    /// For non-selector modules, it usually does not matter which index value
    /// an application assigns to a particular source module, but for selector
    /// modules, the purpose of a source module is defined by its index value.
    /// For example, consider the noise::module::Select noise module, which
    /// requires three source modules.  The control module is the source
    /// module assigned an index value of 2.  The control module determines
    /// whether the noise module will output the value from the source module
    /// assigned an index value of 0 or the output value from the source
    /// module assigned an index value of 1.
    ///
    /// <b>Generating output values with a noise module</b>
    ///
    /// Once an application has connected all required source modules to a
    /// noise module, the application can now begin to generate output values
    /// with that noise module.
    ///
    /// To generate an output value, pass the ( @a x, @a y, @a z ) coordinates
    /// of an input value to the GetValue() method.
    ///
    /// <b>Using a noise module to generate terrain height maps or textures</b>
    ///
    /// One way to generate a terrain height map or a texture is to first
    /// allocate a 2-dimensional array of floating-point values.  For each
    /// array element, pass the array subscripts as @a x and @a y coordinates
    /// to the GetValue() method (leaving the @a z coordinate set to zero) and
    /// place the resulting output value into the array element.
    ///
    /// <b>Creating your own noise modules</b>
    ///
    /// Create a class that publicly derives from noise::module::Module.
    ///
    /// In the constructor, call the base class' constructor while passing the
    /// return value from GetSourceModuleCount() to it.
    ///
    /// Override the GetSourceModuleCount() pure virtual method.  From this
    /// method, return the number of source modules required by your noise
    /// module.
    ///
    /// Override the GetValue() pure virtual method.  For generator modules,
    /// calculate and output a value given the coordinates of the input value.
    /// For other modules, retrieve the output values from each source module
    /// referenced in the protected @a m_pSourceModule array, mathematically
    /// combine those values, and return the combined value.
    ///
    /// When developing a noise module, you must ensure that your noise module
    /// does not modify any source module or control module connected to it; a
    /// noise module can only modify the output value from those source
    /// modules.  You must also ensure that if an application fails to connect
    /// all required source modules via the SetSourceModule() method and then
    /// attempts to call the GetValue() method, your module will raise an
    /// assertion.
    ///
    /// It shouldn't be too difficult to create your own noise module.  If you
    /// still have some problems, take a look at the source code for
    /// noise::module::Add, which is a very simple noise module.
    public abstract class ModuleBase {
        protected ModuleBase[] mSourceModules;

        /// <summary>
        /// Returns the number of source modules required by this noise
        /// module.
        /// </summary>
        public virtual int sourceModuleCount { get { return 0; } }

        /// <summary>
        /// Get or set a reference to a source module connected to this noise.
        /// module.
        /// </summary>
        public ModuleBase this[int ind] {
            get { return mSourceModules != null ? mSourceModules[ind] : null; }
            set { if(mSourceModules != null) mSourceModules[ind] = value; }
        }

        /// <summary>
        /// Generates an output value given the coordinates of the specified
        /// input value.
        /// </summary>
        /// <remarks>
        /// Before an application can call this method, it must first connect
        /// all required source modules via the SetSourceModule() method.  If
        /// these source modules are not connected to this noise module, this
        /// method raises a debug assertion.
        /// </remarks>
        /// <returns>The output value based on location [-1, 1]</returns>
        public abstract float GetValue(float x, float y, float z);

        public float GetValue(Vector3 pos) {
            return GetValue(pos.x, pos.y, pos.z);
        }

        protected ModuleBase() {
            if(sourceModuleCount > 0)
                mSourceModules = new ModuleBase[sourceModuleCount];
        }
    }
}