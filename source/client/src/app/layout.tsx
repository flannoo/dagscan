import type { Metadata } from "next";
import { Inter } from "next/font/google";
import 'leaflet/dist/leaflet.css';
import "./globals.css";
import { ThemeProvider } from "@/lib/providers/themeProvider";
import { Header } from "@/components/header";
import { Footer } from "@/components/footer";
import ReactQueryProvider from "@/lib/providers/reactQueryProvider";
import { Toaster } from "@/components/ui/toaster";
import { cn } from "@/lib/utils";

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
        <head>
          <link rel="icon" type="image/svg+xml" href="/favicon.svg" />
        </head>
        <body>
          <ThemeProvider attribute="class" defaultTheme="system" enableSystem disableTransitionOnChange>
            <div className={cn("w-full bg-yellow-400 text-black text-center py-2 font-semibold")}>
              🚧 This website is in beta and under construction. Some features or data may not be available yet. 🚧
            </div>
            <Header />
            <ReactQueryProvider>
              {children}
            </ReactQueryProvider>
            <Toaster />
            <Footer />
          </ThemeProvider>
        </body>
      </html>
    </>
  )
}
