using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Timers;

namespace Domotica
{
	[Activity (Label = "Domotica", MainLauncher = true, ConfigurationChanges = ( Android.Content.PM.ConfigChanges.Orientation |Android.Content.PM.ConfigChanges.ScreenSize ) ,Icon = "@mipmap/icon", Theme="@style/MyTheme")]
	public class MainActivity : AppCompatActivity
	{
		//UI Variables
		private SupportToolbar mToolbar;
		private MyActionBarDrawerToggle mDrawerToggle;
		private DrawerLayout mDrawerLayout;
		private ListView mDrawer;
		private ArrayAdapter mAdapter;
		private List<string> mDrawerData;
		private SupportFragment mCurrentFragment;
		private Home mHome;
		private Switches1 mSwitches1;
		private Switches2 mSwitches2;
		private Sensors1 mSensors1;
		private Sensors2 mSensors2;
		private Connection1 mConnection1;
		private Mode1 mMode1;
		private Stack<SupportFragment> mStackFragment;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//Ui Items
			mToolbar = FindViewById<SupportToolbar> (Resource.Id.toolbar);
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mDrawer = FindViewById<ListView> (Resource.Id.left_drawer);


			//Fragments
			mHome = new Home();
			mSwitches1 = new Switches1 ();
			mSwitches2 = new Switches2 ();
			mSensors1 = new Sensors1 ();
			mSensors2 = new Sensors2 ();
			mConnection1 = new Connection1 ();
			mMode1 = new Mode1 ();
			mStackFragment = new Stack<SupportFragment> ();

			//Create Toolbar
			SetSupportActionBar (mToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);﻿

			//Create All Fragments
			var trans = SupportFragmentManager.BeginTransaction ();

			trans.Add (Resource.Id.fragmentContainter, mMode1, "Mode1");
			trans.Hide (mMode1);

			trans.Add (Resource.Id.fragmentContainter, mConnection1, "Connection1");
			trans.Hide (mConnection1);

			trans.Add (Resource.Id.fragmentContainter, mSensors2, "Sensors2");
			trans.Hide (mSensors2);

			trans.Add (Resource.Id.fragmentContainter, mSensors1, "Sensors1");
			trans.Hide (mSensors1);

			trans.Add (Resource.Id.fragmentContainter, mSwitches2, "Switches2");
			trans.Hide(mSwitches2);

			trans.Add (Resource.Id.fragmentContainter, mSwitches1, "Switches1");
			trans.Hide (mSwitches1);

			trans.Add (Resource.Id.fragmentContainter, mHome, "Home");

			trans.Commit ();

			mCurrentFragment = mHome;

			//Set Data For the navigation Drawer
			mDrawerData = new List<string> () {"Home", "Switches", "Sensors", "Sensor Threshold", "Timers","Connection", "Modes"};
			//get adapter to interpet list with data
			mAdapter = new ArrayAdapter<string> (this, Resource.Layout.mytextview, mDrawerData);
			mDrawer.Adapter = mAdapter;
			//Enable DrawerToggle
			mDrawerToggle = new MyActionBarDrawerToggle (
				this,
				mDrawerLayout,
				Resource.String.openDrawer,
				Resource.String.closeDrawer
			);

			//Toggle DrawerToggle
			mDrawerLayout.SetDrawerListener (mDrawerToggle);
			SupportActionBar.SetHomeButtonEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);
			mDrawerToggle.SyncState ();

			//if this is the frist time the view is created set title on the toolbar to Home since this this the fragment you're greeted with
			if (savedInstanceState != null)
			{
			}
			else
			{
				SupportActionBar.SetTitle (Resource.String.Home);	
			}

			//Event handlers
			//UI Event Handler
			//if DrawerItem is selected change view and Title on the toolbar acoardingly
			mDrawer.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
			{
				switch(e.Position)
				{
					case 0:
						changeFragment(mHome);
						SupportActionBar.SetTitle (Resource.String.Home);
						break;
					case 1:
						changeFragment(mSwitches1);
						SupportActionBar.SetTitle (Resource.String.Switches1);	
						break;
					case 2:
						changeFragment(mSensors1);
						SupportActionBar.SetTitle (Resource.String.Sensors1);	
						break;
					case 3:
						changeFragment(mSensors2);
						SupportActionBar.SetTitle (Resource.String.Sensors2);	
						break;
					case 4:
						changeFragment(mSwitches2);
						SupportActionBar.SetTitle (Resource.String.Switches2);
						break;
					case 5:
						changeFragment(mConnection1);
						SupportActionBar.SetTitle (Resource.String.Connection);
						break;
					case 6:
						changeFragment(mMode1);
						SupportActionBar.SetTitle (Resource.String.Mode);
						break;
				}
			}; 
		}


		//Get input from drawertoggle
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			mDrawerToggle.OnOptionsItemSelected (item);
			return base.OnOptionsItemSelected (item);
		}

		//Synch drawertoggle with drawerstate
		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mDrawerToggle.SyncState ();
		}

		//if back button is pressed get last seen fragment
		public override void OnBackPressed ()
		{
			if (SupportFragmentManager.BackStackEntryCount > 0) 
			{
				SupportFragmentManager.PopBackStack ();
				mCurrentFragment = mStackFragment.Pop ();
			} 
			else 
			{
				base.OnBackPressed ();
			}
		}

		//Inflate MenuIcons on the Toolbar
		/*public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.action_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}*/


		//Change Shown fragment and hide current
		private void changeFragment(SupportFragment fragment1)
		{
			if (fragment1 != mCurrentFragment) 
			{
				var trans = SupportFragmentManager.BeginTransaction ();
				trans.SetCustomAnimations (Resource.Animation.first_slide_in, Resource.Animation.first_slide_out, Resource.Animation.second_slide_in, Resource.Animation.second_slide_out);
				trans.Hide (mCurrentFragment);
				trans.Show (fragment1);
				trans.AddToBackStack (null);
				trans.Commit ();

				mStackFragment.Push (mCurrentFragment);
				mCurrentFragment = fragment1;
			}
			mDrawerLayout.CloseDrawer (mDrawer);
		}
	}
}


