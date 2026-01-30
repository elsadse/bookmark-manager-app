import logo_light from "@/assets/images/logo-light-theme.svg"
import logo_dark from "@/assets/images/logo-dark-theme.svg"
import { Link } from "react-router"
import { useAuth } from "@/hooks/useAuth"
import { useState } from "react"

export function FormContainerSignIn() {
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const { login } = useAuth()

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault()
        await login(email, password)
    }

    return (
        <div className="flex flex-col gap-y-8 px-5 py-8 md:w-112 md:h-auto md:px-8 md:py-10 rounded-12 bg-neutral-0 dark:bg-neutral-d-800 dark:border border-neutral-d-500">
            <Logo />
            <FormHeader text="Log in your account" supportingText="Welcome back! Please enter your details." />
            <form className="flex flex-col gap-y-4" onSubmit={handleSubmit}>
                <InputField labelInput="Email"
                    typeInput="email"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                />
                <InputField labelInput="Password"
                    typeInput="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                />
                <ButtonForm textButton="Log in" />
            </form>
            <div className="flex flex-col items-center gap-y-3">
                <FormFooterRow textFooterRow="Forgot password?" textFooterRowLink="Reset it" linkFooterRow="/bookmark-manager-app/forgotPassword" />
                <FormFooterRow textFooterRow="Don’t have an account?" textFooterRowLink="Sign up" linkFooterRow="/bookmark-manager-app/signUp" />
            </div>
        </div>
    )
}

export function Logo() {

    return (
        <>
            <img src={logo_light} className="h-8 w-auto flex justify-center dark:hidden" alt="logo" />
            <img src={logo_dark} className="h-8 w-auto flex justify-center hidden dark:block" alt="logo" />
        </>
    )
}

export function FormHeader({ text, supportingText }: { text: string, supportingText: string }) {

    return (
        <div className="flex flex-col gap-y-1.5">
            <span className="text-preset-1">{text}</span>
            <span className="text-preset-4-md text-neutral-800">{supportingText}</span>
        </div>
    )
}

export function InputField({ typeInput, labelInput, value, onChange }: { typeInput: string, labelInput: string, value: string, onChange: (e: React.ChangeEvent<HTMLInputElement>) => void }) {

    return (
        <div className="flex flex-col gap-y-1.5">
            <span className="text-preset-4">{labelInput}</span>
            <input type={typeInput}
                value={value}
                onChange={onChange}
                className="p-3 rounded-8 border border-neutral-500 dark:border-neutral-d-300 dark:bg-neutral-d-600 focus:outline-none
                focus:ring ring-teal-700 cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-500"
            />
        </div>
    )
}

export function ButtonForm({ textButton }: { textButton: string }) {

    return (
        <button className="flex justify-center px-4 py-3 gap-1 bg-teal-700 rounded-8 cursor-pointer ring ring-teal-700" type="submit">
            <span className="text-center px-0.5 text-preset-3 text-neutral-0">{textButton}</span>
        </button>
    )
}

export function FormFooterRow({ textFooterRow, textFooterRowLink, linkFooterRow }: { textFooterRow: string, textFooterRowLink: string, linkFooterRow: string }) {

    return (
        <div className="flex flex-row gap-x-1.5">
            <span className="text-preset-4-md text-neutral-800">{textFooterRow}</span>
            <Link to={linkFooterRow} className="text-preset-4">{textFooterRowLink}</Link>
        </div>
    )
}