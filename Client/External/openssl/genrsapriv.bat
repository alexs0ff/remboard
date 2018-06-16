openssl genrsa -des3 -passout pass:123456 -out private.pem 2048
openssl rsa -in private.pem -outform PEM -passin pass:123456 -pubout -out public.pem