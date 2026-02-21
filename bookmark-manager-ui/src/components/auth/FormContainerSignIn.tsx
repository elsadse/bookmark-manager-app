import logo_light from "@/assets/images/logo-light-theme.svg"
import logo_dark from "@/assets/images/logo-dark-theme.svg"
import LoadingIcon from "@/assets/images/icon-loading.svg"
import { Link } from "react-router"
import { useState, type SyntheticEvent } from "react"
import { useAuthContext } from "@/hooks/useAuthContext"

export function FormContainerSignIn() {
    const [email, setEmail] = useState("azerty@gmail.com")
    const [password, setPassword] = useState("12345678")
    const { login, isLoading, error: { login: error } } = useAuthContext()

    function handleSubmit(event: SyntheticEvent<HTMLFormElement>): void {
        event.preventDefault()
        const formData = new FormData(event.currentTarget)
        login(formData.get("email") as string, formData.get("password") as string)
    }

    return (
        <div className="flex flex-col gap-y-8 px-5 py-8 md:w-112 md:h-auto md:px-8 md:py-10 rounded-12 bg-neutral-0">
            <Logo />
            <FormHeader text="Log in your account" supportingText="Welcome back! Please enter your details." />
            <form className="flex flex-col gap-y-4" onSubmit={handleSubmit}>
                <InputField labelInput="Email"
                    name="email"
                    typeInput="email"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                />
                {
                    error !== null && "errors" in error && "Email" in error.errors &&
                    <div className="flex flex-col gap-y-1.5">
                        {
                            error.errors["Email"].map((error: string, index: number) => (
                                <span key={index} className="text-preset-4 text-red-800">{error}</span>
                            ))
                        }
                    </div>
                }
                <InputField labelInput="Password"
                    name="password"
                    typeInput="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                />
                {
                    error !== null && "errors" in error && "Password" in error.errors &&
                    <div className="flex flex-col">
                        {
                            error.errors["Password"].map((error: string, index: number) => (
                                <span key={index} className="text-preset-4 text-red-800">{error}</span>
                            ))
                        }
                    </div>
                }
                {
                    error !== null && "detail" in error &&
                    <div className="flex flex-col gap-y-1.5">
                        <span className="text-preset-4 text-red-800">{error.detail}</span>
                    </div>
                }
                <ButtonForm textButton="Log in" isLoading={isLoading} />
            </form>
            <div className="flex flex-col items-center gap-y-3">
                <FormFooterRow textFooterRow="Forgot password?" textFooterRowLink="Reset it" linkFooterRow="/forgotPassword" />
                <FormFooterRow textFooterRow="Don’t have an account?" textFooterRowLink="Sign up" linkFooterRow="/register" />
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

export function InputField({ typeInput, labelInput, value, onChange, name }: { name: string, typeInput: string, labelInput: string, value: string, onChange: (e: React.ChangeEvent<HTMLInputElement>) => void }) {

    return (
        <div className="flex flex-col gap-y-1.5">
            <span className="text-preset-4">{labelInput}</span>
            <input type={typeInput}
                name={name}
                value={value}
                onChange={onChange}
                className="p-3 rounded-8 border border-neutral-500 focus:outline-none
                focus:ring ring-teal-700 cursor-pointer hover:bg-neutral-100 dark:hover:bg-neutral-d-500"
            />
        </div>
    )
}

export function ButtonForm({ textButton, isLoading }: { textButton: string, isLoading: boolean }) {

    return (
        <button className="flex justify-center items-center px-4 py-3 gap-1 bg-teal-700 rounded-8 cursor-pointer ring ring-teal-700" type="submit">
            {
                isLoading && <img src={LoadingIcon} alt="Loading Icon" className="w-4 h-4 spin-slow" />
            }
            <span className="text-center px-0.5 text-preset-3 text-neutral-0 dark:text-neutral-d-0">{textButton}</span>
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