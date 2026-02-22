import { ButtonForm, FormFooterRow, FormHeader, InputField, Logo } from "@/components/auth/FormContainerSignIn";
import { useAuthContext } from "@/hooks/useAuthContext";
import { useState, type SyntheticEvent } from "react";

export function FormContainerSignUp() {
    const [fullname, setFullname] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const { register, isLoading, error: { register: error } } = useAuthContext()

    function handleSubmit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault()
        const formData = new FormData(event.currentTarget)
        register(formData.get("fullname") as string, formData.get("email") as string, formData.get("password") as string)
    }

    return (
        <div className="flex flex-col gap-y-8 px-5 py-8 md:w-112 md:h-auto md:px-8 md:py-10 rounded-12 bg-neutral-0 dark:bg-neutral-d-800 dark:border border-neutral-d-500">
            <Logo />
            <FormHeader text="Create your account" supportingText="Join us and start saving your favorite links — organized, searchable, and always within reach." />
            <form className="flex flex-col gap-y-4" onSubmit={handleSubmit}>
                <InputField name="fullname"
                    labelInput="Full name *"
                    typeInput="text"
                    value={fullname}
                    onChange={e => setFullname(e.target.value)}
                    className={`${error !== null && "errors" in error && "Fullname" in error.errors? "border border-red-600 focus:border-none":""}`}
                />
                {
                    error !== null && "errors" in error && "Fullname" in error.errors &&
                    <div className="flex flex-col gap-y-1.5">
                        {
                            error.errors["Fullname"].map((error: string, index: number) => (
                                <span key={index} className="text-preset-4 text-red-800">{error}</span>
                            ))
                        }
                    </div>
                }
                <InputField name="email"
                    labelInput="Email *"
                    typeInput="email"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                    className={`${error !== null && "errors" in error && "Email" in error.errors? "border border-red-600 focus:border-none":""}`}
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
                <InputField name="password"
                    labelInput="Password *"
                    typeInput="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    className={`${error !== null && "errors" in error && "Password" in error.errors? "border border-red-600 focus:border-none":""}`}
                />
                {
                    error !== null && "errors" in error && "Password" in error.errors &&
                    <div className="flex flex-col gap-y-1.5">
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
                <ButtonForm textButton="Create account" isLoading={isLoading} />
            </form>
            <div className="flex flex-col items-center gap-y-3">
                <FormFooterRow textFooterRow="Already have an account?" textFooterRowLink="Log in" linkFooterRow="/login" />
            </div>
        </div>
    )
}