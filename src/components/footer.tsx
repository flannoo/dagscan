"use client";

export function Footer() {
    return (
        <footer className="bg-background border-t py-4 text-center text-sm text-muted-foreground">
            <div className="container mx-auto">
                &copy; {new Date().getFullYear()} Nebula Tech Venture. All rights reserved.
            </div>
        </footer>
    );
}
