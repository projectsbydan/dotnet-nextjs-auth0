import "../styles/globals.css";
import type { AppProps } from "next/app";
import React from "react";
import { Auth0Provider } from "@auth0/auth0-react";
import { ChakraProvider } from "@chakra-ui/react";

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <Auth0Provider
      domain={process.env.NEXT_PUBLIC_AUTH0_DOMAIN || "ENV not configured"}
      clientId={process.env.NEXT_PUBLIC_AUTH0_CLIENTID || "ENV not configured"}
      audience={process.env.NEXT_PUBLIC_AUTH0_AUDIENCE || "ENV not configured"}
      scope="openid"
      redirectUri={
        process.env.NEXT_PUBLIC_AUTH0_REDIRECT || "ENV not configured"
      }
      responseType="token id_token"
    >
      <ChakraProvider>
        <Component {...pageProps} />
      </ChakraProvider>
    </Auth0Provider>
  );
}
export default MyApp;
