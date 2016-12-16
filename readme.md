# XYZ Pointcloud reducer

Creates a copy of a text pointcloud file with reduced precision of the floating point to decrease the file size
http://id144.org

##Usage:
XYZPointcloudReduce [-n:0..9] input_file output_file

###Mandatory arguments:

 input_file     Input point-cloud file containing space separated values as strings. Supported formats: ASC, XYZ, etc. 
 input_file     Output point-cloud file containing space separated values as strings.

###Optional arguments:
 -n:0..9     Specify the count of digits after decimal point, lower number results in lower file size. Default value is 3.
 
Compiled binaries at:
http://id144.org/tools/xyzpointcloudreducer.zip
 
