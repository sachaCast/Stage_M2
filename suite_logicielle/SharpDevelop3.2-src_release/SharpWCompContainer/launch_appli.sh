#!/bin/sh

base_assembly=""

echo "Starting WComp Container ..."

if [ "$1" != "-l" ] || [ -z "$2" ]
then
	echo "Error in parameters: launch_appli.sh -l base_assembly_file.wcc"
	exit 1
else
	base_assembly="$2"
fi

if [ -z "$base_assembly" ]
then
	mono ./Container.exe -n Appli -p 3000 -r "../Beans"
else
	mono ./Container.exe -n Appli -p 3000 -l "$base_assembly" -r "../Beans"
fi
