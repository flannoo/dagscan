import type { Metadata } from "next";
import { Inter } from "next/font/google";
import 'leaflet/dist/leaflet.css';
import "./globals.css";
import { ThemeProvider } from "@/lib/providers/themeProvider";
import { Header } from "@/components/header";
import { Footer } from "@/components/footer";
import ReactQueryProvider from "@/lib/providers/reactQueryProvider";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "DAG Scan",
  description: "DAG Scan is a blockchain explorer for Constellation Network.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <>
      <html lang="en" suppressHydrationWarning>
        <head />
        <body>
          <ThemeProvider attribute="class" defaultTheme="system" enableSystem disableTransitionOnChange>
            <Header />
            <ReactQueryProvider>
              {children}
            </ReactQueryProvider>
            <Footer />
          </ThemeProvider>
        </body>
      </html>
    </>
  )
}
