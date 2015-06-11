using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectPrototype.Test
{
    class Program
    {
        private static KinectSensor sensor;
        private static Skeleton[] skeletons;
        private static byte[] colorImages;

        static void Main(string[] args)
        {
            foreach (var s in KinectSensor.KinectSensors)
            {
                if (s.Status == KinectStatus.Connected)
                {
                    Console.WriteLine("Sensor " + s.UniqueKinectId + " connected.");
                    sensor = s;
                    break;
                }
            }

            Console.WriteLine("A Kinect sensor detected. Press any key to start...");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key != null)
            {
                if (sensor != null)
                {
                    sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    sensor.SkeletonStream.Enable();
                    sensor.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);

                    sensor.Start();
                }
            }

            //sensor.SkeletonFrameReady += sensor_SkeletonFrameReady;
            //sensor.ColorFrameReady += sensor_ColorFrameReady;
            //sensor.DepthFrameReady += sensor_DepthFrameReady;
            sensor.AllFramesReady += sensor_AllFramesReady;

            while (sensor.IsRunning)
            {
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("Kinect will stop after pressing any key");
                    Console.ReadKey();
                    break;
                }
            }

            sensor.Stop();
            Console.WriteLine("Kinect stopped. Press enter to exit");
            key = Console.ReadKey();
             if (key.Key == ConsoleKey.Enter)
             {
                 return;
             }
        }

        static void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            bool receivedSkeletonData = false;
            bool receivedColorImageData = false;

            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
            {
                if (SFrame == null)
                {
                    // The image processing took too long. More than 2 frames behind.
                }
                else
                {
                    skeletons = new Skeleton[SFrame.SkeletonArrayLength];
                    SFrame.CopySkeletonDataTo(skeletons);
                    receivedSkeletonData = true;
                }
            }



            using (ColorImageFrame ciFrame = e.OpenColorImageFrame())
            {
                if (ciFrame == null)
                {
                    
                }
                else
                {
                    colorImages = new byte[ciFrame.PixelDataLength];
                    ciFrame.CopyPixelDataTo(colorImages);
                    receivedColorImageData = true;
                }
            }

            if (receivedSkeletonData && receivedColorImageData)
            {
                Skeleton currentSkeleton = (from s in skeletons
                                            where s.TrackingState ==
                                            SkeletonTrackingState.Tracked
                                            select s).FirstOrDefault();

                if (currentSkeleton != null && colorImages != null)
                {
                    Console.Clear();
                    Console.WriteLine("Skeleton-x: " + currentSkeleton.Position.X +
                        "\r\nSkeleton-y: " + currentSkeleton.Position.Y +
                        "\r\nSkeleton-z: " + currentSkeleton.Position.Z +
                        "\r\n======================\r\n");// + 
                        //"ColorImagePixelBytes:\r\n" + System.Text.Encoding.UTF8.GetString(colorImages));
                }
            }


        }

        static void sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {

        }

        static void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            bool receivedData = false;

            using (ColorImageFrame ciFrame = e.OpenColorImageFrame())
            {
                if (ciFrame == null)
                {
                    
                }
                else
                {
                    colorImages = new byte[ciFrame.PixelDataLength];
                    ciFrame.CopyPixelDataTo(colorImages);
                    receivedData = true;
                }

                if (receivedData)
                {

                }
            }
        }

        static void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            bool receivedData = false;

            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
            {
                if (SFrame == null)
                {
                    // The image processing took too long. More than 2 frames behind.
                }
                else
                {
                    skeletons = new Skeleton[SFrame.SkeletonArrayLength];
                    SFrame.CopySkeletonDataTo(skeletons);
                    receivedData = true;
                }
            }

            if (receivedData)
            {
                Skeleton currentSkeleton = (from s in skeletons
                                            where s.TrackingState ==
                                            SkeletonTrackingState.Tracked
                                            select s).FirstOrDefault();

                if (currentSkeleton != null)
                {
                    Console.Clear();
                    Console.WriteLine("Skeleton-x: " + currentSkeleton.Position.X + 
                        "\r\nSkeleton-y: " + currentSkeleton.Position.Y + 
                        "\r\nSkeleton-z: " + currentSkeleton.Position.Z + 
                        "\r\n======================");

                    //SetEllipsePosition(head,
                    //  currentSkeleton.Joints[JointType.Head]);
                    //SetEllipsePosition(leftHand,
                    //  currentSkeleton.Joints[JointType.HandLeft]);
                    //SetEllipsePosition(rightHand,
                    //  currentSkeleton.Joints[JointType.HandRight]);
                    //SetEllipsePosition(rightFoot,
                    //  currentSkeleton.Joints[JointType.FootRight]);
                    //SetEllipsePosition(leftFoot,
                    //  currentSkeleton.Joints[JointType.FootLeft]);
                }
            }
        }


    }
}
