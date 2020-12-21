
Commands to generate keys:
 * private key:
 ```
    openssl genpkey -algorithm RSA -out private_key.pem
 ```
 * public key:
 ```
    openssl rsa -in private_key.pem -outform PEM -pubout -out public_key.pem
 ```

 Package used to read public key pem file: [PemUtils](https://github.com/huysentruitw/pem-utils)
