using System;
using System.Collections.ObjectModel;
using CoreGraphics;
using Foundation;
using Syncfusion.SfSchedule.iOS;
using UIKit;

namespace RecurrenceExceptions
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        NSDate exceptionDate3;
        UIButton removeExceptionDates;
        UIButton addExceptionDates;
        UIButton addExceptionAppointment;
        UIButton removeExceptionAppointment;
        ObservableCollection<ScheduleAppointment> scheduleAppointmentCollection = new ObservableCollection<ScheduleAppointment>();
        CGRect CurrentFrame;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.CurrentFrame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);
            this.AddButton();
            //Creating an instance for SfSchedule control
            SFSchedule schedule = new SFSchedule();
            schedule.Frame = new CGRect(0, removeExceptionAppointment.Frame.Bottom, this.CurrentFrame.Size.Width, this.CurrentFrame.Size.Height - removeExceptionAppointment.Frame.Bottom);
            schedule.ScheduleView = SFScheduleView.SFScheduleViewWeek;

            NSCalendar calendar = new NSCalendar(NSCalendarType.Gregorian);
            NSDate today = NSDate.Now;
            // Get the year, month, day from the date
            NSDateComponents startDateComponents = calendar.Components(NSCalendarUnit.Year |
                                                                       NSCalendarUnit.Month |
                                                                       NSCalendarUnit.Day, today);
            // Set the year, month, day, hour, minute, second
            startDateComponents.Year = 2017;
            startDateComponents.Month = 09;
            startDateComponents.Day = 03;
            startDateComponents.Hour = 10;
            startDateComponents.Minute = 0;
            startDateComponents.Second = 0;

            //setting start time for the event
            NSDate startDate = calendar.DateFromComponents(startDateComponents);

            //setting end time for the event
            NSDate endDate = startDate.AddSeconds(2 * 60 * 60);

            // set moveto date to schedule
            schedule.MoveToDate(startDate);

            // Set the exception dates. 
            var exceptionDate1 = startDate;
            var exceptionDate2 = startDate.AddSeconds(2 * 24 * 60 * 60);
            exceptionDate3 = startDate.AddSeconds(4 * 24 * 60 * 60);

            // Add Schedule appointment
            ScheduleAppointment recurrenceAppointment = new ScheduleAppointment();
            recurrenceAppointment.StartTime = startDate;
            recurrenceAppointment.EndTime = endDate;
            recurrenceAppointment.Subject = (NSString)"Occurs Daily";
            recurrenceAppointment.AppointmentBackground = UIColor.Blue;
            recurrenceAppointment.RecurrenceRule = (NSString)"FREQ=DAILY;COUNT=20";
            recurrenceAppointment.RecurrenceExceptionDates = new System.Collections.ObjectModel.ObservableCollection<NSDate> {
                exceptionDate1,
                exceptionDate2,
                exceptionDate3
            };

            scheduleAppointmentCollection.Add(recurrenceAppointment);
            schedule.ItemsSource = scheduleAppointmentCollection;
            this.View.AddSubview(schedule);
        }

        private void AddButton()
        {
            addExceptionDates = new UIButton();
            addExceptionDates.SetTitle("AddExceptionDates", UIControlState.Normal);
            addExceptionDates.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            addExceptionDates.Frame = new CoreGraphics.CGRect(0, 0, this.CurrentFrame.Size.Width, 50);
            addExceptionDates.TouchUpInside += AddExceptionDates_TouchUpInside;
            this.View.AddSubview(addExceptionDates);

            removeExceptionDates = new UIButton();
            removeExceptionDates.SetTitle("RemoveExceptionDates", UIControlState.Normal);
            removeExceptionDates.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            removeExceptionDates.Frame = new CoreGraphics.CGRect(0, addExceptionDates.Frame.Bottom, this.CurrentFrame.Size.Width, 50);
            removeExceptionDates.TouchUpInside += RemoveExceptionDates_TouchUpInside;
            this.View.AddSubview(removeExceptionDates);

            addExceptionAppointment = new UIButton();
            addExceptionAppointment.SetTitle("AddExceptionAppointment", UIControlState.Normal);
            addExceptionAppointment.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            addExceptionAppointment.Frame = new CoreGraphics.CGRect(0, removeExceptionDates.Frame.Bottom, this.CurrentFrame.Size.Width, 50);
            addExceptionAppointment.TouchUpInside += AddExceptionAppointment_TouchUpInside;
            this.View.AddSubview(addExceptionAppointment);

            removeExceptionAppointment = new UIButton();
            removeExceptionAppointment.SetTitle("RemoveExceptionAppointment", UIControlState.Normal);
            removeExceptionAppointment.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            removeExceptionAppointment.Frame = new CoreGraphics.CGRect(0, addExceptionAppointment.Frame.Bottom, this.CurrentFrame.Size.Width, 50);
            removeExceptionAppointment.TouchUpInside += RemoveExceptionAppointment_TouchUpInside;
            this.View.AddSubview(removeExceptionAppointment);
        }

        private void RemoveExceptionAppointment_TouchUpInside(object sender, EventArgs e)
        {
            if (scheduleAppointmentCollection.Count > 1)
            {
                var exceptioAppointment = scheduleAppointmentCollection[1];
                scheduleAppointmentCollection.Remove(exceptioAppointment);
            }
        }


        private void AddExceptionAppointment_TouchUpInside(object sender, EventArgs e)
        {
            var recurrenceAppointment = scheduleAppointmentCollection[0];

            // Add appointment to current recurrence series.
            ScheduleAppointment exceptionAppointment = new ScheduleAppointment();
            exceptionAppointment.StartTime = exceptionDate3.AddSeconds(2 * 60 * 60);
            exceptionAppointment.EndTime = exceptionAppointment.StartTime.AddSeconds(2 * 60 * 60);
            exceptionAppointment.Subject = (NSString)"Occurs Daily";
            exceptionAppointment.AppointmentBackground = UIColor.Red;
            exceptionAppointment.RecurrenceId = recurrenceAppointment;
            exceptionAppointment.ExceptionOccurrenceActualDate = exceptionDate3;
            scheduleAppointmentCollection.Add(exceptionAppointment);
        }


        private void RemoveExceptionDates_TouchUpInside(object sender, EventArgs e)
        {
            scheduleAppointmentCollection[0].RecurrenceExceptionDates.RemoveAt(0);
        }


        private void AddExceptionDates_TouchUpInside(object sender, EventArgs e)
        {
            var removeDate = exceptionDate3.AddSeconds(24 * 60 * 60);
            scheduleAppointmentCollection[0].RecurrenceExceptionDates.Add(removeDate);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
