# Description: This script takes a picture sequence with the NoIR
# camera. You need put a button in the GPIO port to trigger the 
# camera. Also, you have to put a led to see the script status.
# Author: Ing. Edward U. Benitez Rendon
# Date: 06-05-17

from picamera import PiCamera
import time
import RPi.GPIO as GPIO
import os

# GPIO pin for the LED indicator
led = 24
# GPIO pin for the trigger button
btnStart = 5
status = False
# Default path to create folders for photos
imgFolderPath = '/home/pi/Cam/Pictures/'

def startScript(channel):
	global status
	status = True

# Create a name folder to save all photos taken by the raspi camera
def nameFolder():
	day = time.strftime("%d")
	month = time.strftime("%m")
	year = time.strftime("%y")
	hour = time.strftime("%H")
	min = time.strftime("%M")
	sec = time.strftime("%S")
	data0 = day + '_' + month + '_' + year +'_'
	data1 = hour + '_' + min + '_' + sec
	_nameFolder = imgFolderPath + data0 + data1 + '/'
	os.mkdir(_nameFolder)
	return _nameFolder

GPIO.setmode(GPIO.BCM)
GPIO.setup(led, GPIO.OUT)
GPIO.setup(btnStart, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.add_event_detect(btnStart, GPIO.FALLING, callback=startScript)
GPIO.output(led, 0)

camera = PiCamera()

while True:
	if status == True:
		subFolder = nameFolder()
		print "Taking 50 pictures"
		GPIO.output(led, 1)
		camera.start_preview()
		for i in range(10):
		  time.sleep(.1)
		  camera.capture( subFolder + 'image%s.jpg' % i)
		camera.stop_preview()
		GPIO.output(led, 0)
		print "End Script"
		status = False
		time.sleep(1)
