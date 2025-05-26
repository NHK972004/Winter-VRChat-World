using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.Rendering;

namespace UdonVR.Spooky
{
    public class Door : UdonSharpBehaviour
    {
        public Animator door;
        public string boolName = "anim";

        public bool useInteract = true;
        public bool useAudio = true;

        public AudioSource doorAudio;
        public AudioClip audioClipOpen;
        public AudioClip audioClipClose;

        public OcclusionPortal occlusionPortal; // reference to Occlusion Portal component

        public GameObject toggledObject; // GameObject to toggle with the door's state

        private void Start()
        {
            DisableInteractive = !useInteract;
        }

        public override void OnPickupUseDown()
        {
            Interact();
        }

        public override void OnPickupUseUp()
        {
            Interact();
        }

        public override void Interact()
        {
            if (!Networking.LocalPlayer.isLocal) return; // Ensure the interaction is local

            bool currentState = door.GetBool(boolName);

            if (currentState)
            {
                DoorClose(); // If the door is currently open, close it
            }
            else
            {
                DoorOpen(); // If the door is currently closed, open it
            }
        }

        public void DoorOpen()
        {
            door.SetBool(boolName, true);
            PlayClip(audioClipOpen);

            if (occlusionPortal != null) // Check if OcclusionPortal reference is assigned
            {
                occlusionPortal.open = true; // Open the OcclusionPortal when the door opens
            }

            ToggleObjectState(true); // Set the GameObject to active
        }

        public void DoorClose()
        {
            door.SetBool(boolName, false);
            PlayClip(audioClipClose);

            if (occlusionPortal != null) // Check if OcclusionPortal reference is assigned
            {
                occlusionPortal.open = false; // Close the OcclusionPortal when the door closes
            }

            ToggleObjectState(false); // Set the GameObject to inactive
        }

        private void PlayClip(AudioClip clip)
        {
            if (useAudio)
            {
                doorAudio.Stop();
                doorAudio.clip = clip;
                doorAudio.Play();
            }
        }

        // Function to toggle the GameObject's active state based on the passed state
        private void ToggleObjectState(bool state)
        {
            if (toggledObject != null)
            {
                toggledObject.SetActive(state);
            }
        }
    }
}
