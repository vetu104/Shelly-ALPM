#!/bin/bash
# This script allows debugging the application as root using pkexec.
# It is intended to be used as a custom executable in a Rider Run/Debug Configuration.

if [ "$#" -lt 1 ]; then
    echo "Usage: $0 <executable> [arguments...]"
    exit 1
fi

pkexec env DISPLAY=$DISPLAY XAUTHORITY=$XAUTHORITY "$@"
