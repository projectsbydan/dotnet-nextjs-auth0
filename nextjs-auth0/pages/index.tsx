import { useAuth0 } from "@auth0/auth0-react";
import {
  Button,
  Container,
  Heading,
  HStack,
  Stack,
  Text,
} from "@chakra-ui/react";
import React, { useState } from "react";

export default function Home() {
  const {
    isLoading,
    isAuthenticated,
    error,
    user,
    logout,
    getAccessTokenSilently,
    loginWithPopup,
  } = useAuth0();
  const [result, setResult] = useState("");

  const getApiData = async () => {
    try {
      const accessToken = await getAccessTokenSilently();
      var call = await fetch("http://localhost:5000/auth/getdata", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
        mode: "cors",
      });
      setResult(await call.text());
    } catch {
      setResult("Something didn't work");
    }
  };

  const getUsers = async () => {
    try {
      const accessToken = await getAccessTokenSilently();
      var call = await fetch("http://localhost:5000/auth/getusers", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
        mode: "cors",
      });
      setResult(await call.text());
    } catch {
      setResult("Something didn't work");
    }
  };

  return (
    <Stack justify="center" h="100vh">
      <Container maxW="container.md">
        <Heading mb={6}>Auth0 authentification with .net Api</Heading>
        <HStack spacing={12}>
          <Button
            colorScheme="blue"
            disabled={isLoading}
            onClick={() => loginWithPopup()}
            w="full"
          >
            Login
          </Button>
          <Button onClick={() => logout()} variant="outline" w="full">
            Logout
          </Button>
        </HStack>

        <Heading size="md" mt={12}>
          Logged in users E-Mail Address
        </Heading>
        <Text>{user?.email || "no E-Mail Address"}</Text>
        <Text color="gray.300">
          Account is {!isAuthenticated && <strong>not</strong>} authentificated
        </Text>

        <Stack spacing={4} mt={12}>
          <Heading size="md">Data from the .net Api</Heading>
          <HStack spacing={12}>
            <Button w="full" onClick={getApiData}>
              Get data from API
            </Button>
            <Button w="full" onClick={getUsers}>
              Get Users from Auth0 through the API
            </Button>
          </HStack>

          <Text>{result ? result : "Nothing loaded"}</Text>
        </Stack>

        <Stack spacing={4} mt={12}>
          <Heading size="md">Debbuging messages</Heading>
          <Text>Errors from Auth: {error?.message || "all good ✔"}</Text>
        </Stack>
      </Container>
    </Stack>
  );
}
