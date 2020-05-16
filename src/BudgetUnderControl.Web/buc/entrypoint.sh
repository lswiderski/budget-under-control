#!/bin/sh
for file in /app/dist/js/app.*.js;
do
  if [ ! -f $file.tmpl.js ]; then
    cp $file $file.tmpl.js
  fi
  envsubst '$VUE_APP_BACKEND_API_URL' < $file.tmpl.js > $file
done
echo "Starting http-server"
http-server dist