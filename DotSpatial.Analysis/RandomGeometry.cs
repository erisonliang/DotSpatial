﻿// *************************************************
// DotSpatial.Analysis function to create random points in a polygon
// Dan Ames, 2/26/2013
// **************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class for supporting methods to generate random points.
    /// </summary>
    public static class RandomGeometry
    {
        /// <summary>
        /// Creates a specified number of random point features inside multiple polygon features in a feature set. 
        /// </summary>
        /// <param name="ConstrainingFeatures">Random points will be generated inside all features in this feature set.</param>
        /// <param name="NumberOfPoints">The number of points to be randomly generated.</param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet RandomPoints(FeatureSet ConstrainingFeatures, long NumberOfPoints)
        {
            //This function generates random points within the boundaries of all polygon features in a feature set
            FeatureSet fsOut = new FeatureSet();
            fsOut.FeatureType = FeatureType.Point;
            Coordinate c = new Coordinate();
            Random r = new Random();
            long i = 0;
            while (i < NumberOfPoints)
            {
                c = new Coordinate();
                //make a random point somewhere in the rectangular extents of the feature set
                double rndx = r.Next(0, 100000) / 100000.0;
                double rndy = r.Next(0, 100000) / 100000.0;
                c.X = rndx * (ConstrainingFeatures.Extent.MaxX - ConstrainingFeatures.Extent.MinX) + ConstrainingFeatures.Extent.MinX;
                c.Y = rndy * (ConstrainingFeatures.Extent.MaxY - ConstrainingFeatures.Extent.MinY) + ConstrainingFeatures.Extent.MinY;

                //check if the point falls within the polygon featureset
                foreach (Feature f in ConstrainingFeatures.Features)
                {
                    if (f.Intersects(c))
                    {
                        fsOut.AddFeature(new Feature(c));
                        i++;
                    }
                }
            }
            return fsOut;
        }
        /// <summary>
        /// Creates a specified number of random point features inside a single polygon feature. 
        /// </summary>
        /// <param name="ConstrainingFeature">Random points will be generated inside this polygon feature.</param>
        /// <param name="NumberOfPoints">The number of points to be randomly generated.</param>
        /// <returns>A point feature set with the randomly created features.</returns>
        public static FeatureSet RandomPoints(Feature ConstrainingFeature, long NumberOfPoints)
        {
            //This function generates random points within the boundaries of one polygon feature
            FeatureSet fsOut = new FeatureSet();
            fsOut.FeatureType = FeatureType.Point;
            Coordinate c = new Coordinate();
            Random r = new Random();
            long i = 0;
            while (i < NumberOfPoints)
            {
                c = new Coordinate();
                //make a random point somewhere in the rectangular extents of the feature
                double rndx = r.Next(0, 100000) / 100000.0;
                double rndy = r.Next(0, 100000) / 100000.0;
                c.X = rndx * (ConstrainingFeature.Envelope.Right() - ConstrainingFeature.Envelope.Left()) + ConstrainingFeature.Envelope.Left();
                c.Y = rndy * (ConstrainingFeature.Envelope.Top() - ConstrainingFeature.Envelope.Bottom()) + ConstrainingFeature.Envelope.Bottom();
                //check if the point falls within the polygon featureset
                if (ConstrainingFeature.Intersects(c))
                {
                    fsOut.AddFeature(new Feature(c));
                    i++;
                }
            }
            return fsOut;
        }        
    }
}